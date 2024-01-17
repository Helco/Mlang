using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;


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
using static Mlang.Language.ShaderCompiler;

namespace Mlang.Language;

public partial class VariantCompiler : IDisposable
{
    private readonly SourceFile sourceFile;
    private readonly string[] extraDownstreamOptions = Array.Empty<string>();
    private readonly List<Diagnostic> diagnostics = new();
    private readonly Dictionary<ASTDeclaration, LayoutInfo> layoutInfos = new();
    private readonly ASTTranslationUnit unit;
    private readonly ASTOption[] options;
    private readonly ShaderInfo shaderInfo;

#if DEBUG
    public bool ThrowInternalErrors { get; set; } = false;
#endif
    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;
    public bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);
    public bool OutputGeneratedSourceOnError { get; set; } = false;

    internal IDownstreamCompiler DownstreamCompiler { get; set; } = new SilkShadercDownstreamCompiler();

    internal VariantCompiler(SourceFile sourceFile, ASTTranslationUnit unit, ShaderInfo shaderInfo)
    {
        this.sourceFile = sourceFile;
        this.unit = unit;
        this.shaderInfo = shaderInfo;
        options = unit.Blocks.OfType<ASTOption>().ToArray();
    }

    public void Dispose() => DownstreamCompiler.Dispose();

    public void ClearDiagnostics() => diagnostics.Clear();

    public ShaderVariant? CompileVariant(IReadOnlyDictionary<string, uint>? optionValues_ = null) =>
        CompileVariant(new DictionaryOptionValueSet(optionValues_ ?? new Dictionary<string, uint>()));

    internal ShaderVariant? CompileVariant(IOptionValueSet userOptionValues, ShaderVariant? programInvariant = null)
    {
        ShaderVariantKey variantKey = new(shaderInfo.SourceHash, 0);
        try
        {
            return CompileVariantIgnoringErrorOutput(userOptionValues, programInvariant, out variantKey);
        }
        catch (Exception e)
#if DEBUG
        when (!ThrowInternalErrors)
#endif
        {
            diagnostics.Add(DiagInternal(e));
        }
        finally
        {
            if (diagnostics.Any())
                diagnostics.Insert(0, DiagStartOfVariant(shaderInfo.FormatVariantName(variantKey)));
        }
        return null;
    }

    private ShaderVariant? CompileVariantIgnoringErrorOutput(IOptionValueSet userOptionValues, ShaderVariant? programInvariant, out ShaderVariantKey variantKey)
    {
        var optionValues = new FilteredOptionValueSet(options, userOptionValues);
        var optionBits = optionValues.CollectOptionBits(options);
        variantKey = new ShaderVariantKey(shaderInfo.SourceHash, optionBits);
        var pipelineState = ComposePipelineState(optionValues);

        if (programInvariant != null)
        {
            if (shaderInfo.GetProgramInvariantKey(variantKey) != shaderInfo.GetProgramInvariantKey(programInvariant.VariantKey))
                throw new ArgumentException($"Given program invariant is not valid for requested variant");
            return programInvariant.AsProgramInvariant(variantKey, pipelineState);
        }

        var vertexStageBlock = FindStageBlock(TokenKind.KwVertex, optionValues);
        var fragmentStageBlock = FindStageBlock(TokenKind.KwFragment, optionValues);
        if (vertexStageBlock == null || fragmentStageBlock == null)
            return null;

        layoutInfos.Clear();
        var transferredInstanceVars = FindUsedInstanceVariables(fragmentStageBlock, optionValues);
        var vertexAttributes = LayoutVertexAttributes(optionValues);
        var (bindingSetSizes, bindings) = LayoutBindings(optionValues);
        LayoutVarying(optionValues, transferredInstanceVars);

        using var vertexGLSL = new StringWriter(CultureInfo.InvariantCulture);
        using var fragmentGLSL = new StringWriter(CultureInfo.InvariantCulture);
        var glslContext = new GLSLOutputContext()
        {
            Pipeline = pipelineState,
            OptionValues = optionValues,
            TransferredInstanceVars = transferredInstanceVars,
            LayoutInfos = layoutInfos
        };
        var vertexVisitor = new GLSLVertexOutputVisitor(vertexStageBlock, vertexGLSL, glslContext);
        var fragmentVisitor = new GLSLFragmentOutputVisitor(fragmentStageBlock, fragmentGLSL, glslContext);
        unit.Visit(vertexVisitor);
        unit.Visit(fragmentVisitor);

        var macros = CollectMacros(optionValues);
        var vertexBytes = CompileStage(vertexGLSL.ToString(), TokenKind.KwVertex, macros);
        var fragmentBytes = CompileStage(fragmentGLSL.ToString(), TokenKind.KwFragment, macros);
        return HasError ? null : new(
            variantKey,
            pipelineState,
            vertexAttributes,
            bindingSetSizes,
            bindings,
            vertexBytes!,
            fragmentBytes!);
    }

    private PipelineState ComposePipelineState(IOptionValueSet optionValues)
    {
        var state = PipelineState.GetDefault(1);
        foreach (var block in unit.Blocks.OfType<ASTPipelineBlock>())
        {
            if (!block.EvaluateCondition(optionValues))
                continue;
            state = state.With(block.State);
        }
        return state;
    }

    private KeyValuePair<string, string>[] CollectMacros(IOptionValueSet optionValues)
    {
        var allOptions = unit.Blocks.OfType<ASTOption>().ToArray();
        return allOptions
            .Select(opt => new KeyValuePair<string, string>(
                opt.Name,
                (optionValues.TryGetValue(opt.Name, out var value) ? value : 0).ToString()))
            .Concat(allOptions
                .Where(opt => opt.NamedValues?.Length > 0)
                .SelectMany(opt => opt.NamedValues!
                    .Select((name, index) => new KeyValuePair<string, string>(name, index.ToString()))))
            .ToArray();
    }

    private byte[]? CompileStage(
        string source,
        TokenKind stageKind,
        KeyValuePair<string, string>[] macros)
    {
        var result = DownstreamCompiler.Compile(source, stageKind, macros, extraDownstreamOptions);
        if (OutputGeneratedSourceOnError && result.HasError)
            diagnostics.Add(DiagGeneratedSource(source));
        diagnostics.AddRange(result.Diagnostics);
        return result.HasError ? null : result.Result.ToArray();
    }

    private ASTStageBlock? FindStageBlock(TokenKind stageKind, IOptionValueSet optionValues)
    {
        if (unit == null)
            return null;
        var stageBlocks = unit.Blocks
                .OfType<ASTStageBlock>()
                .Where(b => b.Stage == stageKind && b.EvaluateCondition(optionValues))
                .ToArray() as IEnumerable<ASTStageBlock>;
        if (stageBlocks.None())
        {
            diagnostics.Add(DiagNoStageBlock(stageKind));
            return null;
        }
        if (stageBlocks.Count() > 1)
        {
            var firstBlock = stageBlocks.First();
            var secondBlock = stageBlocks.Skip(1).First();
            diagnostics.Add(DiagMultipleStageBlocks(sourceFile, firstBlock.Range, secondBlock.Range, stageKind));
            return null;
        }
        return stageBlocks.Single();
    }

    private Dictionary<string, ASTDeclaration> FindAllInstanceVariables(IOptionValueSet optionValues)
    {
        var list = unit!.Blocks
            .OfType<ASTStorageBlock>()
            .Where(b => b.StorageKind == TokenKind.KwInstances && b.EvaluateCondition(optionValues))
            .SelectMany(b => b.Declarations);
        var map = new Dictionary<string, ASTDeclaration>();
        foreach (var decl in list)
        {
            if (map.TryGetValue(decl.Name, out var prevDecl))
                diagnostics.Add(DiagDuplicateStorageName(sourceFile, prevDecl, decl));
            else
                map.Add(decl.Name, decl);
        }
        return map;
    }

    private HashSet<ASTDeclaration> FindUsedInstanceVariables(ASTStageBlock fragmentStage, IOptionValueSet optionValues)
    {
        if (unit == null || !optionValues.GetBool(IsInstancedOptionName))
            return new();
        var allDecls = FindAllInstanceVariables(optionValues);
        var visitor = new VariableUsageVisitor<ASTDeclaration>(allDecls);

        foreach (var function in unit.Blocks.OfType<ASTFunction>())
            function.Visit(visitor);
        fragmentStage.Visit(visitor);

        return visitor.UsedVariables.Select(name => allDecls[name]).ToHashSet();
    }
}
