using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mlang.Model;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Text;

using static Mlang.Diagnostics;

namespace Mlang.Language;

public partial class Compiler
{
    internal const int MaxVariantBits = 16;
    internal const string IsInstancedOptionName = "IsInstanced";

    private readonly SourceFile sourceFile;
    private readonly Lexer lexer;
    private readonly Parser parser;
    private readonly List<Diagnostic> diagnostics = new();
    internal ASTTranslationUnit? unit;
    private bool wasParsed;

#if DEBUG
    public bool ThrowInternalErrors { get; set; } = true;
#endif
    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;
    public bool HasError => diagnostics.Any(d => d.Severity == Severity.Error || d.Severity == Severity.InternalError);

    public Compiler(string fileName, string sourceText) : this(fileName, new StringReader(sourceText)) { }

    public Compiler(string fileName, TextReader reader)
    {
        sourceFile = new SourceFile(fileName, reader);
        lexer = new(sourceFile);
        parser = new(lexer) { SourceFile = sourceFile };
    }

    public bool ParseShader()
    {
        if (wasParsed)
            return !HasError;
        wasParsed = true;

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

        return !HasError;
    }

    public string? CompileVariant(IReadOnlyDictionary<string, uint>? optionValues_ = null)
    {
        if (!ParseShader())
            return null;

        var userOptionValues = new DictionaryOptionValueSet(optionValues_ ?? new Dictionary<string, uint>());
        var optionValues = new FilteredOptionValueSet(unit!.Blocks.OfType<ASTOption>().ToArray(), userOptionValues);
        var pipelineState = ComposePipelineState(optionValues);

        using var vertexGLSL = new StringWriter();
        using var fragmentGLSL = new StringWriter();
        WriteGLSL(pipelineState, vertexGLSL, fragmentGLSL, optionValues);

        return vertexGLSL.ToString() + "\n------------------------------------\n\n" + fragmentGLSL.ToString();
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
}
