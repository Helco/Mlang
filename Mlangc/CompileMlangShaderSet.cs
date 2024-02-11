using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mlang.Language;
using Yoakke.SynKit.Reporting.Present;
using Yoakke.SynKit.Text;
using Mlang;
using Mlang.Model;
using System.Collections.Concurrent;

namespace Mlangc;

public class CompileMlangShaderSet
{
    public string[] ShaderFiles { get; set; } = Array.Empty<string>();

    public string OutputPath { get; set; } = "output.shadercache";

    public bool OutputGeneratedSourceOnError { get; set; } = false;
    public bool EmbedShaderSource { get; set; } = false;
    public bool RunInParallel { get; set; } = true;

    private List<Diagnostic> diagnostics = new();

    private bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);

    public bool Execute()
    {
        var forOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = RunInParallel ? -1 : 1
        };
        var shaderCompilers = Array.Empty<ShaderCompiler>();
        var wasSuccessful = false;
        try
        {
            shaderCompilers = ShaderFiles.Select(CreateShaderCompiler).ToArray();
            if (HasError)
                return false;

            Parallel.ForEach(shaderCompilers, forOptions, shaderCompiler =>
            {
                shaderCompiler.ParseShader();
                if (shaderCompiler.Diagnostics.Any())
                {
                    lock (diagnostics)
                        diagnostics.AddRange(shaderCompiler.Diagnostics);
                    shaderCompiler.ClearDiagnostics();
                }
            });
            if (HasError)
                return false;

            var allProgramVariants = shaderCompilers.SelectMany(
                (c, i) => c.ProgramVariants.Select(v => (shaderCompiler: c, shaderCompilerI: i, variantOptions: v)));
            var variantCounts = new int[shaderCompilers.Length];
            var allVariants = new List<ShaderVariant>(allProgramVariants.Count());
            int totalCompiled = 0;

            Parallel.ForEach(allProgramVariants, forOptions, (t, loopState) =>
            {
                if (!t.shaderCompiler.ProgramInvariantsFor(t.variantOptions).Any())
                    return;

                Interlocked.Increment(ref totalCompiled);
                using var variantCompiler = t.shaderCompiler.CreateVariantCompiler();
                variantCompiler.OutputGeneratedSourceOnError = OutputGeneratedSourceOnError;
                var baseVariant = variantCompiler.CompileVariant(t.variantOptions);
                if (variantCompiler.Diagnostics.Any())
                {
                    lock (diagnostics)
                        diagnostics.AddRange(variantCompiler.Diagnostics);
                }
                variantCompiler.ClearDiagnostics();
                if (baseVariant == null)
                    return;

                foreach (var invariantOptions in t.shaderCompiler.ProgramInvariantsFor(t.variantOptions))
                {
                    var invariant = variantCompiler.CompileVariant(invariantOptions, baseVariant);
                    if (invariant == null)
                    {
                        lock (diagnostics)
                            throw new Exception("Unexpectedly invariant could not be compiled");
                    }
                    else
                    {
                        Interlocked.Increment(ref variantCounts[t.shaderCompilerI]);
                        lock (allVariants)
                            allVariants.Add(invariant);
                    }
                }
            }); // TODO: Reuse downstream compiler per thread

            using var setWriter = new ShaderSetFileWriter(
                new FileStream(OutputPath, FileMode.Create, FileAccess.Write));
            for (int i = 0; i < shaderCompilers.Length; i++)
            {
                var shaderCompiler = shaderCompilers[i];
                var taskItem = ShaderFiles[i];
                var name = Path.GetFileNameWithoutExtension(taskItem);
                var source = EmbedShaderSource ? File.ReadAllText(taskItem) : null; // TODO: Reuse alread read source
                setWriter.AddShader(shaderCompiler.ShaderInfo!, name, source, variantCounts[i]);
            }

            foreach (var variant in allVariants)
                setWriter.WriteVariant(variant);

            wasSuccessful = !HasError;
            Console.WriteLine($"Compiled {totalCompiled} and assembled {allVariants.Count} shader variants in total");
        }
        catch (IOException e)
        {
            diagnostics.Add(DiagShaderSetIOError(OutputPath, e.Message));
        }
        finally
        {
            foreach (var diagnostic in diagnostics)
                ReportDiagnostic(diagnostic);
            foreach (var compiler in shaderCompilers)
                compiler?.Dispose();
            if (!wasSuccessful)
                File.Delete(OutputPath);
        }
        return wasSuccessful;
    }

    private ShaderCompiler CreateShaderCompiler(string item)
    {
        try
        {
            return new ShaderCompiler(item, new FileStream(item, FileMode.Open, FileAccess.Read))
#if DEBUG
            { ThrowInternalErrors = true }
#endif
            ;
        }
        catch (IOException e)
        {
            diagnostics.Add(DiagShaderIOError(item, e.Message));
        }
        return null!;
    }

    private struct MSBuildDiagnosticLocation
    {
        public string? file;
        public int lineNumber, columnNumber, endLineNumber, endColumnNumber;

        public MSBuildDiagnosticLocation(Location location)
        {
            file = location.File.Path;
            if (location.Range.Start == location.Range.End)
            {
                if (location.Range.Start != default)
                {
                    lineNumber = endLineNumber = location.Range.Start.Line + 1;
                    columnNumber = endColumnNumber = location.Range.Start.Column + 1;
                }
            }
            else
            {
                lineNumber = location.Range.Start.Line + 1;
                columnNumber = location.Range.Start.Column + 1;
                endLineNumber = location.Range.End.Line + 1;
                endColumnNumber = location.Range.End.Column; // SynKit uses exclusive end, MSBuild uses inclusive
            }
        }
    }

    private TextDiagnosticsPresenter Presenter = new TextDiagnosticsPresenter(Console.Error);
    private void ReportDiagnostic(Diagnostic diagnostic)
    {
        Presenter.Present(diagnostic.ConvertToSynKit());
    }

    public static readonly DiagnosticCategory CategoryInternal = new("MSBUILD");

    public static readonly DiagnosticType TypeShaderIOError = CategoryInternal.Create(
        Severity.Error, "Could not open file: {0}");

    public static Diagnostic DiagShaderIOError(string fileName, string errorMessage)
    {
        var sourceFile = new SourceFile(fileName, TextReader.Null);
        var sourceInfo = new Location(sourceFile, default);
        return TypeShaderIOError.Create([errorMessage], [sourceInfo]);
    }

    public static readonly DiagnosticType TypeShaderSetIOError = CategoryInternal.Create(
        Severity.Error, "Failed to write shader set: {0}");

    public static Diagnostic DiagShaderSetIOError(string fileName, string errorMessage)
    {
        var sourceFile = new SourceFile(fileName, TextReader.Null);
        var sourceInfo = new Location(sourceFile, default);
        return TypeShaderIOError.Create([errorMessage], [sourceInfo]);
    }
}
