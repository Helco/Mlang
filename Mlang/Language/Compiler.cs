using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mlang.Model;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Text;

using static Mlang.Diagnostics;

namespace Mlang.Language;

internal interface IDownstreamCompilationResult
{
    bool HasError { get; }
    IReadOnlyCollection<Diagnostic> Diagnostics { get; }
    ReadOnlySpan<byte> Result { get; }
}

internal interface IDownstreamCompiler
{
    IDownstreamCompilationResult Compile(
        string source,
        TokenKind stage,
        IEnumerable<KeyValuePair<string, string>> macros,
        IEnumerable<string> extraOptions);
}

public partial class Compiler : IDisposable
{
    private const int BufferSize = 1024; // the documented default StreamReader buffer
    internal const int MaxVariantBits = 16;
    internal const string IsInstancedOptionName = "IsInstanced";

    private readonly Stream sourceStream; // disposed (if at all) by sourceFile
    private readonly SourceFile sourceFile;
    private readonly Lexer lexer;
    private readonly Parser parser;
    private readonly List<Diagnostic> diagnostics = new();
    private readonly IDownstreamCompiler downstreamCompiler = new SilkShadercDownstreamCompiler();
    private readonly string[] extraDownstreamOptions = Array.Empty<string>();
    internal ASTTranslationUnit? unit;
    private bool? parseSuccess;
    private bool disposedValue;
    private uint? sourceHash;

#if DEBUG
    public bool ThrowInternalErrors { get; set; } = true;
#endif
    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;
    public bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);
    public bool OutputGeneratedSourceOnError { get; set; } = true;

    public Compiler(string fileName, string sourceText) :
        this(fileName, new MemoryStream(Encoding.UTF8.GetBytes(sourceText))) { }

    public Compiler(string fileName, Stream stream, bool disposeStream = true)
    {
        sourceStream = stream;
        var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, BufferSize, leaveOpen: !disposeStream);
        sourceFile = new SourceFile(fileName, reader);
        lexer = new(sourceFile);
        parser = new(lexer) { SourceFile = sourceFile };
    }

    public void ClearDiagnostics() => diagnostics.Clear();

    public bool ParseShader()
    {
        if (parseSuccess.HasValue)
            return parseSuccess.Value;
        parseSuccess = false;

        try
        {
            var result = parser.ParseTranslationUnit();
            diagnostics.AddRange(parser.Diagnostics);
            if (!result.IsOk)
            {
                var errToken = (IToken<TokenKind>)result.Error.Got!;
                if (errToken.Kind == TokenKind.Error)
                    diagnostics.Add(DiagLexer(sourceFile, errToken));
                else
                    diagnostics.Add(DiagParser(sourceFile, errToken, result.Error.Elements.Values));
                return false;
            }
            unit = result.Ok.Value;

            CheckVariantSpace();
            CheckNamedOptionValues();
            CheckOptionConditions();
            CheckSpecialOptions();
        }
        catch (Exception e)
#if DEBUG
        when (!ThrowInternalErrors)
#endif
        {
            diagnostics.Add(DiagInternal(e));
        }

        parseSuccess = !HasError;
        return parseSuccess.Value;
    }

    public ShaderVariant? CompileVariant(IReadOnlyDictionary<string, uint>? optionValues_ = null)
    {
        if (!ParseShader())
            return null;

        var userOptionValues = new DictionaryOptionValueSet(optionValues_ ?? new Dictionary<string, uint>());
        var optionValues = new FilteredOptionValueSet(unit!.Blocks.OfType<ASTOption>().ToArray(), userOptionValues);
        var pipelineState = ComposePipelineState(optionValues);

        using var vertexGLSL = new StringWriter();
        using var fragmentGLSL = new StringWriter();
        WriteGLSL(pipelineState, vertexGLSL, fragmentGLSL, optionValues);

        var macros = CollectMacros(optionValues);
        var vertexBytes = CompileStage(vertexGLSL.ToString(), TokenKind.KwVertex, macros);
        var fragmentBytes = CompileStage(fragmentGLSL.ToString(), TokenKind.KwFragment, macros);

        var shaderHash = HashShaderSource();
        var optionBits = CollectOptionBits(optionValues);
        var variantKey = new ShaderVariantKey(shaderHash, optionBits);
        return HasError ? null : new(variantKey, pipelineState, vertexBytes!, fragmentBytes!);
    }

    private PipelineState ComposePipelineState(IOptionValueSet optionValues)
    {
        var state = PipelineState.GetDefault(1);
        foreach (var block in unit!.Blocks.OfType<ASTPipelineBlock>())
        {
            if (!block.EvaluateCondition(optionValues))
                continue;
            state = state.With(block.State);
        }
        return state;
    }

    private KeyValuePair<string, string>[] CollectMacros(IOptionValueSet optionValues)
    {
        var allOptions = unit!.Blocks.OfType<ASTOption>().ToArray();
        return allOptions
            .Select(opt => new KeyValuePair<string, string>(
                opt.Name,
                (optionValues.TryGetValue(opt.Name, out var value) ? value : 0).ToString()))
            .Concat(allOptions
                .Where(opt => opt.NamedValues?.Length > 0)
                .SelectMany(opt => opt.NamedValues
                    .Select((name, index) => new KeyValuePair<string, string>(name, index.ToString()))))
            .ToArray();
    }

    private byte[]? CompileStage(
        string source,
        TokenKind stageKind,
        KeyValuePair<string, string>[] macros)
    {
        var result = downstreamCompiler.Compile(source, stageKind, macros, extraDownstreamOptions);
        if (OutputGeneratedSourceOnError && result.HasError)
            diagnostics.Add(DiagGeneratedSource(source));
        diagnostics.AddRange(result.Diagnostics);
        return result.HasError ? null : result.Result.ToArray();
    }

    private uint HashShaderSource()
    {
        if (sourceHash == null)
        {
            sourceStream.Position = 0;
            var crc32 = new System.IO.Hashing.Crc32();
            crc32.Append(sourceStream);
            sourceHash = crc32.GetCurrentHashAsUInt32();
        }
        return sourceHash.Value;
    }

    private uint CollectOptionBits(IOptionValueSet optionValues)
    {
        var allOptions = unit!.Blocks.OfType<ASTOption>().ToArray();
        var bits = 0u;
        foreach (var option in allOptions)
        {
            uint value;
            optionValues.TryGetValue(option.Name, out value);
            bits |= (value & ((1u << option.BitCount) - 1u)) << option.BitOffset;
        }
        return bits;
    }

    private void CheckVariantSpace()
    {
        var lastOption = unit?.Blocks.OfType<ASTOption>().LastOrDefault();
        if (lastOption == null)
            return;
        int totalVariantBits = lastOption.BitOffset + lastOption.BitCount;
        if (totalVariantBits >= MaxVariantBits)
        {
            diagnostics.Add(DiagVariantSpaceTooLarge(totalVariantBits, MaxVariantBits));
            return;
        }
    }

    private void CheckNamedOptionValues()
    {
        if (unit == null)
            return;
        var options = new Dictionary<string, ASTOption>();
        foreach (var option in unit.Blocks.OfType<ASTOption>())
        {
            if (options.TryGetValue(option.Name, out var prevOption))
                diagnostics.Add(DiagDuplicateOptionName(sourceFile, prevOption, option));
            else
                options.Add(option.Name, option);
        }
        
        var values = new Dictionary<string, (uint, ASTOption)>();
        foreach (var option in unit.Blocks.OfType<ASTOption>())
        {
            if (option.NamedValues == null)
                continue;
            for (uint i = 0; i < option.NamedValues.Length; i++)
            {
                var name = option.NamedValues[i];
                if (options.TryGetValue(name, out var prevOption))
                    diagnostics.Add(DiagOptionNameIsValue(sourceFile, prevOption, option, name));

                if (!values.TryGetValue(name, out var prevValue))
                    values.Add(name, (i, option));
                else if (prevValue.Item1 != i)
                    diagnostics.Add(DiagDuplicateNamedValue(sourceFile, prevValue.Item2, option, name));
            }
        }
    }

    private void CheckOptionConditions()
    {
        var optionValueSet = new FilteredOptionValueSet(unit!.Blocks.OfType<ASTOption>().ToArray());
        foreach (var block in unit.Blocks.OfType<ASTConditionalGlobalBlock>())
        {
            if (block.Condition == null)
                continue;

            optionValueSet.AccessedOption = false;
            if (!block.Condition.TryOptionEvaluate(optionValueSet, out var value))
                diagnostics.Add(DiagOptionConditionNotEvaluable(sourceFile, block.Condition.Range));
            else if (!optionValueSet.AccessedOption)
                diagnostics.Add(DiagOptionConditionIsConstant(sourceFile, block.Condition.Range, value));
        }
    }

    private void CheckSpecialOptions()
    {
        if (unit == null)
            return;

        var isInstanced = unit.Blocks.OfType<ASTOption>().FirstOrDefault(b => b.Name == IsInstancedOptionName);
        if (isInstanced?.NamedValues != null)
            diagnostics.Add(DiagSpecialOptionWithValues(sourceFile, isInstanced));

        if (isInstanced == null)
        {
            var instancesBlock = unit.Blocks.FirstOrDefault(b => b is ASTStorageBlock { StorageKind: TokenKind.KwInstances });
            if (instancesBlock != null)
                diagnostics.Add(DiagInstancesBlockWithoutOption(sourceFile, instancesBlock.Range));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                sourceFile.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
