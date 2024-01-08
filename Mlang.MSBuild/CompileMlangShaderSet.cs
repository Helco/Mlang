using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mlang.Language;
using Yoakke.SynKit.Text;
using static Mlang.MSBuild.MSBuildDiagnostics;

namespace Mlang.MSBuild;

public class CompileMlangShaderSet : Task
{
    [Required]
    public ITaskItem[] ShaderFiles { get; set; } = Array.Empty<ITaskItem>();

    [Required]
    public string OutputPath { get; set; } = "output.shadercache";

    public bool OutputGeneratedSourceOnError { get; set; } = false;
    public bool EmbedShaderSource { get; set; } = false;

    private List<Diagnostic> diagnostics = new();

    private bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);

    public override bool Execute()
    {
        var shaderCompilers = Array.Empty<Compiler>();
        var wasSuccessful = false;
        try
        {
            using var setWriter = new ShaderSetFileWriter(
                new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write));

            shaderCompilers = ShaderFiles.Select(CreateShaderCompiler).ToArray();
            if (HasError)
                return false;

            foreach (var shaderCompiler in shaderCompilers)
            {
                shaderCompiler.ParseShader();
                diagnostics.AddRange(shaderCompiler.Diagnostics);
                shaderCompiler.ClearDiagnostics();
            }
            if (HasError)
                return false;

            int totalVariants = 0;
            foreach (var (shaderCompiler, taskItem) in shaderCompilers.Zip(ShaderFiles, (a, b) => (a, b)))
            {
                var name = Path.GetFileNameWithoutExtension(taskItem.ItemSpec);
                var source = EmbedShaderSource ? File.ReadAllText(taskItem.ItemSpec) : null; // TODO: Reuse alread read source
                setWriter.AddShader(shaderCompiler.ShaderInfo!, name, source, shaderCompiler.AllVariants.Count);
                totalVariants += shaderCompiler.AllVariants.Count;
            }

            foreach (var shaderCompiler in shaderCompilers)
            {
                foreach (var variantOptions in shaderCompiler.AllVariants)
                {
                    var variant = shaderCompiler.CompileVariant(variantOptions);
                    if (variant != null)
                        setWriter.WriteVariant(variant);
                    diagnostics.AddRange(shaderCompiler.Diagnostics);
                    shaderCompiler.ClearDiagnostics();
                }
                if (HasError)
                    return false;
            }

            wasSuccessful = true;
            ReportDiagnosticBySeverity(Severity.Info, "MLANG0000", default, "Compiled {0} shader variants in total", [totalVariants]);
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
        return !HasError;
    }

    private Compiler CreateShaderCompiler(ITaskItem item)
    {
        try
        {
            return new Compiler(item.ItemSpec, new FileStream(item.ItemSpec, FileMode.Open, FileAccess.Read));
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
