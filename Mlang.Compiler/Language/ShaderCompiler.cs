using System;
using System.Collections;
using System.Collections.Generic;
#if NETSTANDARD2_0
using System.Collections.Generic.Polyfill;
#endif
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

internal interface IDownstreamCompiler : IDisposable
{
    IDownstreamCompilationResult Compile(
        string source,
        TokenKind stage,
        IEnumerable<KeyValuePair<string, string>> macros,
        IEnumerable<string> extraOptions);
}

public partial class ShaderCompiler : IDisposable
{
    private const int BufferSize = 1024; // the documented default StreamReader buffer
    internal const int MaxVariantBits = 16;
    internal const string IsInstancedOptionName = "IsInstanced";
    internal const string TransferredInstancePrefix = "instance_";

    private readonly Stream sourceStream; // disposed (if at all) by sourceFile
    private readonly SourceFile sourceFile;
    private readonly Lexer lexer;
    private readonly Parser parser;
    private readonly List<Diagnostic> diagnostics = new();
    internal ASTTranslationUnit? unit;
    private VariantCollection? allVariants, programVariants;
    private bool? parseSuccess;
    private bool disposedValue;
    private uint? sourceHash;

#if DEBUG
    public bool ThrowInternalErrors { get; set; } = false;
#endif
    internal IReadOnlyCollection<IOptionValueSet> AllVariants =>
        allVariants ??= new(unit!.Blocks.OfType<ASTOption>());
    internal IReadOnlyCollection<IOptionValueSet> ProgramVariants =>
        programVariants ??= new(unit!.Blocks.OfType<ASTOption>(), onlyWithVariance: false);
    internal IReadOnlyCollection<IOptionValueSet> ProgramInvariantsFor(IOptionValueSet valueSet)
    {
        var optionBits = valueSet.CollectOptionBits(unit!.Blocks.OfType<ASTOption>());
        var baseOptionBits = optionBits & ~ShaderInfo!.ProgramInvarianceMask;
        return new VariantCollection(unit!.Blocks.OfType<ASTOption>(), onlyWithVariance: true, baseOptionBits);
    }

    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;
    public bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);
    public ShaderInfo? ShaderInfo { get; private set; }

    public ShaderCompiler(string fileName, string sourceText) :
        this(fileName, new MemoryStream(Encoding.UTF8.GetBytes(sourceText))) { }

    public ShaderCompiler(string fileName, Stream stream, bool disposeStream = true)
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
            CheckOptionProgramInvariance();
            CollectShaderInfo();
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

    public VariantCompiler CreateVariantCompiler()
    {
        if (ShaderInfo == null || unit == null || HasError)
            throw new InvalidOperationException("Cannot create variant compiler without succesfully compiling shader first");
        return new VariantCompiler(sourceFile, unit, ShaderInfo);
    }

    internal IEnumerable<string> AllVariantNames() => AllVariants.Select(FormatVariantName);

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

    internal string FormatVariantName(IOptionValueSet optionValues)
    {
        if (ShaderInfo == null || unit == null)
            return "<unknown variant>";
        var optionBits = optionValues.CollectOptionBits(unit.Blocks.OfType<ASTOption>());
        return ShaderInfo.FormatVariantName(new(ShaderInfo.SourceHash, optionBits));
    }

    private void CollectShaderInfo()
    {
        if (unit == null)
            return;
        var options = unit.Blocks.OfType<ASTOption>()
            .Select(opt => new OptionInfo(opt.Name, opt.NamedValues))
            .ToArray();
        var programInvarianceMask = 0u;
        foreach (var block in unit.Blocks.OfType<ASTOption>().Where(b => b.IsProgramInvariant))
            programInvarianceMask |= ((1u << block.BitCount) - 1) << block.BitOffset;

        var attributes = new HashSet<string>();
        var instances = new HashSet<string>();
        var bindings = new HashSet<string>();
        foreach (var block in unit.Blocks.OfType<ASTStorageBlock>())
        {
            var list = block.StorageKind switch
            {
                TokenKind.KwAttributes => attributes,
                TokenKind.KwInstances => instances,
                TokenKind.KwUniform => bindings,
                _ => null
            };
            list?.UnionWith(block.Declarations.Select(d => d.Name));
            if (block.StorageKind == TokenKind.KwInstances)
                bindings.Add(block.NameForReflection);
        }
        ShaderInfo = new()
        {
            SourceHash = HashShaderSource(),
            ProgramInvarianceMask = programInvarianceMask,
            Options = options,
            VertexAttributes = attributes.ToArray(),
            InstanceAttributes = instances.ToArray(),
            Bindings = bindings.ToArray()
        };
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

    private void CheckOptionProgramInvariance()
    {
        var potentiallyVariantOptions = unit?.Blocks.OfType<ASTOption>().ToDictionary(o => o.Name, o => o);
        if (potentiallyVariantOptions == null || potentiallyVariantOptions.None())
            return;
        var visitor = new OptionProgramVarianceVisitor(potentiallyVariantOptions);
        unit!.Visit(visitor);

        // all remaining options do not effect the shader programs
        foreach (var option in potentiallyVariantOptions.Values)
            option.IsProgramInvariant = true;
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
