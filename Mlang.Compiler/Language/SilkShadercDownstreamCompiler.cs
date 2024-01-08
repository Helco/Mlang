using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Yoakke.SynKit.Text;
using Silk.NET.Shaderc;
using Silk.NET.Core.Loader;
using Silk.NET.Core.Contexts;
using ShadercCompiler = Silk.NET.Shaderc.Compiler;
using ShadercCompileOptions = Silk.NET.Shaderc.CompileOptions;
using ShadercCompilationResult = Silk.NET.Shaderc.CompilationResult;
using TextRange = Yoakke.SynKit.Text.Range;

namespace Mlang.Language;

internal unsafe class SilkShadercDownstreamCompiler : IDownstreamCompiler
{
    private readonly Shaderc api = CreateShadercApi();

    private static readonly DiagnosticCategory CategoryShaderc = new("Shaderc");
    private static readonly DiagnosticType TypeGLSLError = CategoryShaderc.Create(Severity.Error, "{0}");
    private static readonly DiagnosticType TypeGLSLWarning = CategoryShaderc.Create(Severity.Warning, "{0}");
    private static readonly DiagnosticType TypeGLSLInternalError = CategoryShaderc.Create(Severity.InternalError, "{0}");
    private static readonly DiagnosticType TypeGLSLUnknownSeverity = CategoryShaderc.Create(Severity.Error, "(unknown severity {1}): {0}");
    private static readonly DiagnosticType TypeUnknownError = CategoryShaderc.Create(
        Severity.Error, "No error message, shaderc returned {0}");

    public IDownstreamCompilationResult Compile(string source, TokenKind stage, IEnumerable<KeyValuePair<string, string>> macros, IEnumerable<string> extraOptions)
    {
        ShadercCompiler* compiler = null;
        ShadercCompileOptions* compileOptions = null;
        ShadercCompilationResult* shadercResult = null;

        try
        {
            var sourceBytes = Encoding.UTF8.GetBytes(source);
            var shaderKind = stage switch
            {
                TokenKind.KwVertex => ShaderKind.VertexShader,
                TokenKind.KwFragment => ShaderKind.FragmentShader,
                _ => throw new ArgumentException($"Invalid or unsupported stage for Shaderc downstream compiler: {stage}")
            };

            compileOptions = api.CompileOptionsInitialize();
            api.CompileOptionsSetSourceLanguage(compileOptions, SourceLanguage.Glsl);
            api.CompileOptionsSetAutoBindUniforms(compileOptions, false);
            api.CompileOptionsSetAutoMapLocations(compileOptions, false);
            api.CompileOptionsSetOptimizationLevel(compileOptions, OptimizationLevel.Performance);
            api.CompileOptionsSetGenerateDebugInfo(compileOptions);
            api.CompileOptionsSetTargetEnv(compileOptions, TargetEnv.Vulkan, (uint)EnvVersion.Vulkan12);
            api.CompileOptionsSetTargetSpirv(compileOptions, SpirvVersion.Shaderc14);
            var utf8Macros = macros.Select(p => (Encoding.UTF8.GetBytes(p.Key), Encoding.UTF8.GetBytes(p.Value))).ToArray();
            foreach (var (key, value) in utf8Macros)
                api.CompileOptionsAddMacroDefinition(compileOptions, key, (nuint)key.Length, value, (nuint)value.Length);

            compiler = api.CompilerInitialize();
            shadercResult = api.CompileIntoSpv(compiler, sourceBytes, (nuint)sourceBytes.Length, shaderKind, "mlang"u8, "main"u8, compileOptions);
            var status = api.ResultGetCompilationStatus(shadercResult);

            var bytes = Array.Empty<byte>();
            if (status == CompilationStatus.Success)
            {
                var span = new ReadOnlySpan<byte>(
                    api.ResultGetBytes(shadercResult),
                    checked((int)api.ResultGetLength(shadercResult)));
                bytes = span.ToArray();
            }

            var countDiagnostics = api.ResultGetNumWarnings(shadercResult) + api.ResultGetNumErrors(shadercResult);
            var diagnostics = new List<Diagnostic>((int)countDiagnostics);
            var errorMessage = api.ResultGetErrorMessageS(shadercResult) ?? "";
            ParseDiagnostics(source, diagnostics, errorMessage);
            if (status != CompilationStatus.Success && diagnostics.None())
                diagnostics.Add(TypeUnknownError.Create([status]));

            return new Result(bytes, diagnostics);
        }
        finally
        {
            if (compiler != null)
                api.CompilerRelease(compiler);
            if (compileOptions != null)
                api.CompileOptionsRelease(compileOptions);
            if (shadercResult != null)
                api.ResultRelease(shadercResult);
        }
    }

    private static readonly Regex DiagnosticHeaderRegex = new(@"(?:\n|^)\w+:(?:(\d+):)? ([ \w]+): ", RegexOptions.Compiled);

    private void ParseDiagnostics(string source, List<Diagnostic> diagnostics, string errorMessage)
    {
        var sourceFile = new SourceFile("glsl", source);

        var matches = DiagnosticHeaderRegex.Matches(errorMessage);
        for (int i = 0; i < matches.Count; i++)
        {
            var startIndex = matches[i].Index + matches[i].Length;
            var endIndex = i + 1 < matches.Count ? matches[i + 1].Index : errorMessage.Length;
            var message = errorMessage.Substring(startIndex, endIndex - startIndex).Trim();

            var sourceInfos = Array.Empty<Location>();
            if (matches[i].Groups[1].Success && int.TryParse(matches[i].Groups[1].Value, out var lineNumber))
            {
                var range = ParseErrorRange(sourceFile, message, lineNumber - 1);
                sourceInfos = [new(sourceFile, range)];
            }

            diagnostics.Add(matches[i].Groups[2].Value.ToLowerInvariant() switch
            {
                "error" => TypeGLSLError.Create([message], sourceInfos),
                "warning" => TypeGLSLWarning.Create([message], sourceInfos),
                "internal error" => TypeGLSLInternalError.Create([message], sourceInfos),
                var severity => TypeGLSLUnknownSeverity.Create([message, severity], sourceInfos)
            });
        }
    }

    private TextRange ParseErrorRange(ISourceFile sourceFile, string message, int lineNumber)
    {
        var endI = message.IndexOf('\'', 1);
        if (message.StartsWith("'") && endI > 1)
        {
            // this will not always be correct, but whatever...
            var searchTerm = message.Substring(1, endI - 1);
            var line = sourceFile.GetLine(lineNumber);
            var column = line.IndexOf(searchTerm);
            if (column >= 0 && line.IndexOf(searchTerm, column + searchTerm.Length + 1) < 0)
                return new(new Position(lineNumber, column), new Position(lineNumber, column + searchTerm.Length));
        }
        return new(new Position(lineNumber, 0), new Position(lineNumber + 1, 0));
    }

    private class Result : IDownstreamCompilationResult
    {
        private readonly byte[] bytes;
        public IReadOnlyCollection<Diagnostic> Diagnostics { get; }

        public bool HasError => Diagnostics.Any(d => d.Severity is Severity.Error or Severity.InternalError);
        ReadOnlySpan<byte> IDownstreamCompilationResult.Result => bytes;

        public Result(byte[] bytes, IReadOnlyCollection<Diagnostic> diagnostics)
        {
            this.bytes = bytes;
            Diagnostics = diagnostics;
        }
    }

    private static Shaderc CreateShadercApi()
    {
        try
        {
            return Shaderc.GetApi();
        }
        catch(FileNotFoundException)
        {
            // Silk looks at many different places, but not enough for the custom MSBuild task
            // unfortunately the name container is internal and we need it for modifying where
            // it looks for the runtimes folder
            // We inject the candidate for tasks/netstandard2.0/native-library.so then 
            // Silk will create the correct candidate tasks/netstandard2.0/runtimes/... for us            
            var shadercAssembly = typeof(Shaderc).Assembly;
            var shadercNameContainer = shadercAssembly!.CreateInstance(
                "Silk.NET.Shaderc.ShadercLibraryNameContainer") as SearchPathContainer
                ?? throw new InvalidOperationException("Could not create instance of shaderc library name container type");
            var pathResolver = new DefaultPathResolver();
            var shadercPath = Path.Combine(Path.GetDirectoryName(shadercAssembly.Location)!, "alskdjalsdkj") ;
            pathResolver.Resolvers.Insert(0, name =>
            {
                var shadercPath = Path.Combine(Path.GetDirectoryName(shadercAssembly.Location)!, name) ;
                return new[] { shadercPath };
            });
            var libraryLoader = LibraryLoader.GetPlatformDefaultLoader();
            foreach (var name in shadercNameContainer.GetLibraryNames())
            {
                if (UnmanagedLibrary.TryCreate(name, libraryLoader, pathResolver, out var library))
                    return new Shaderc(new DefaultNativeContext(library));
            }

            throw;
        }
    }
}
