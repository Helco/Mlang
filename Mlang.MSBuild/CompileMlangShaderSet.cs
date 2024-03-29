﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mlang;
using Mlang.Language;
using Mlang.Model;
using Yoakke.SynKit.Text;
using static Mlang.MSBuild.MSBuildDiagnostics;

namespace Mlang.MSBuild;

public class CompileMlangShaderSet : Microsoft.Build.Utilities.Task
{
    [Required]
    public ITaskItem[] ShaderFiles { get; set; } = Array.Empty<ITaskItem>();

    [Required]
    public string OutputPath { get; set; } = "output.shadercache";

    public bool OutputGeneratedSourceOnError { get; set; } = false;
    public bool EmbedShaderSource { get; set; } = false;
    public bool RunInParallel { get; set; } = true;

    private List<Diagnostic> diagnostics = new();

    private bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);

    public override bool Execute()
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
                    lock(diagnostics)
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
                            ReportDiagnosticBySeverity(Severity.InternalError, "MLANG0000", default, "Unexpectedly invariant could not be compiled", []);
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
                var name = Path.GetFileNameWithoutExtension(taskItem.ItemSpec);
                var source = EmbedShaderSource ? File.ReadAllText(taskItem.ItemSpec) : null; // TODO: Reuse alread read source
                setWriter.AddShader(shaderCompiler.ShaderInfo!, name, source, variantCounts[i]);
            }

            foreach (var variant in allVariants)
                setWriter.WriteVariant(variant);

            wasSuccessful = !HasError;
            var totalPrograms = shaderCompilers.Sum(c => c.ProgramVariants.Count);
            var totalVariants = shaderCompilers.Sum(c => c.AllVariants.Count);
            ReportDiagnosticBySeverity(Severity.Info, "MLANG0000", default,
                "Compiled {0}/{2} and assembled {1}/{3} shader variants in total",
                [totalCompiled, allVariants.Count, totalPrograms, totalVariants]);
        }
        catch(IOException e)
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

    private ShaderCompiler CreateShaderCompiler(ITaskItem item)
    {
        try
        {
            return new ShaderCompiler(item.ItemSpec, new FileStream(item.ItemSpec, FileMode.Open, FileAccess.Read));
        }
        catch(IOException e)
        {
            diagnostics.Add(DiagShaderIOError(item.ItemSpec, e.Message));
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

    private void ReportDiagnostic(Diagnostic diagnostic)
    {
        var code = diagnostic.Type.Code;
        if (!code.StartsWith("mlang", StringComparison.InvariantCultureIgnoreCase))
            code = "MLANG-" + code;
        MSBuildDiagnosticLocation location = default;
        if (diagnostic.SourceInfos.Any())
            location = new(diagnostic.SourceInfos.First());
        var messageArgs = diagnostic.MessageParams.Select(p => p ?? "<null>").ToArray();
        ReportDiagnosticBySeverity(diagnostic.Severity, code, location, diagnostic.Type.Message, messageArgs);

        for (int i = 1; i < diagnostic.SourceInfos.Count; i++)
        {
            var sourceInfoMessage = "see also:";
            if (i < diagnostic.Type.SourceInfoMessages.Count)
                sourceInfoMessage = $"see also {diagnostic.Type.SourceInfoMessages[i]}: ";
            ReportDiagnosticBySeverity(Severity.Info, "MLANG-SEEALSO", new(diagnostic.SourceInfos[i]), sourceInfoMessage, messageArgs);
        }

        if (diagnostic.Type.FootNote != null)
            ReportDiagnosticBySeverity(Severity.Info, "MLANG-FOOTNOTE", location: default, diagnostic.Type.FootNote, messageArgs);
    }

    private void ReportDiagnosticBySeverity(
        Severity severity,
        string code,
        MSBuildDiagnosticLocation location,
        string message,
        object[] messageArgs)
    {
        switch(severity)
        {
            case Severity.Error:
                Log.LogError(
                    subcategory: null,
                    code,
                    helpKeyword: null,
                    location.file,
                    location.lineNumber, location.columnNumber,
                    location.endLineNumber, location.endColumnNumber,
                    message, messageArgs);
                break;
            case Severity.InternalError:
                Log.LogCriticalMessage(
                    subcategory: null,
                    code,
                    helpKeyword: null,
                    location.file,
                    location.lineNumber, location.columnNumber,
                    location.endLineNumber, location.endColumnNumber,
                    message, messageArgs);
                break;
            case Severity.Warning:
                Log.LogWarning(
                    subcategory: null,
                    code,
                    helpKeyword: null,
                    location.file,
                    location.lineNumber, location.columnNumber,
                    location.endLineNumber, location.endColumnNumber,
                    message, messageArgs);
                break;
            case Severity.Info:
            default:
                Log.LogMessage(
                    subcategory: null,
                    code,
                    helpKeyword: null,
                    location.file,
                    location.lineNumber, location.columnNumber,
                    location.endLineNumber, location.endColumnNumber,
                    MessageImportance.High,
                    message, messageArgs);
                break;
        }
    }
}
