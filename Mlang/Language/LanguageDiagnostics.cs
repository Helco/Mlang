using System;
using System.Collections.Generic;
using System.Linq;
using Mlang.Language;
using Yoakke.SynKit.Parser;
using Yoakke.SynKit.Text;
using TextRange = Yoakke.SynKit.Text.Range;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang;

partial class Diagnostics
{
    internal static readonly DiagnosticCategory CategoryLanguage = new("MLANG");

    internal static readonly DiagnosticType TypeLexer = CategoryLanguage.Error("Found invalid token");
    internal static Diagnostic DiagLexer(ISourceFile source, Tk token) =>
        TypeLexer.Create(sourceInfos: [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeParser = CategoryLanguage.CreateWithFootNote(
        Severity.Error, "Could not parse {0} at this point", "{1}");
    internal static Diagnostic DiagParser(ISourceFile source, Tk token, IEnumerable<ParseErrorElement> elements) =>
        TypeParser.Create(new object[]
        {
            token.Kind,
            string.Join("\n", elements.Select(e => $"For {e.Context} expected {string.Join(" or ", e.Expected)}"))
        }, new[] { new Location(source, token.Range) });

    internal static readonly DiagnosticType TypeTooFewOptions = CategoryLanguage.Create(
        Severity.Error, "An option needs at least two values");

    internal static Diagnostic DiagTooFewOptions(ISourceFile source, TextRange range) =>
        TypeTooFewOptions.Create(sourceInfos: [new(source, range)]);

    internal static readonly DiagnosticType TypeUnknownPipelineFactState = CategoryLanguage.Create(
       Severity.Error, "Unknown pipeline fact state: {0}");

    internal static Diagnostic DiagUnknownPipelineFactState(ISourceFile source, Tk token) =>
        TypeUnknownPipelineFactState.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownPipelineStateForArgument = CategoryLanguage.Create(
        Severity.Error, "Unknown pipeline state for single argument: {0}");

    internal static Diagnostic DiagUnknownPipelineStateForArgument(ISourceFile source, Tk token) =>
        TypeUnknownPipelineStateForArgument.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownPipelineStateForTwoArguments = CategoryLanguage.Create(
        Severity.Error, "Unknown pipeline state for two arguments: {0}");

    internal static Diagnostic DiagUnknownPipelineStateForTwoArguments(ISourceFile source, Tk token) =>
        TypeUnknownPipelineStateForTwoArguments.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownPipelineStateForVector = CategoryLanguage.Create(
        Severity.Error, "Unknown pipeline state for vector argument: {0}");

    internal static Diagnostic DiagUnknownPipelineStateForVector(ISourceFile source, Tk token) =>
        TypeUnknownPipelineStateForVector.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeExpectedBooleanValue = CategoryLanguage.Create(
        Severity.Error, "Expected boolean value for pipeline state {0} instead got {1}");

    internal static Diagnostic DiagExpectedBooleanValue(ISourceFile source, TextRange range, string context, string token) =>
        TypeExpectedBooleanValue.Create([context, token], [new(source, range)]);

    internal static readonly DiagnosticType TypeExpectedIntegerValue = CategoryLanguage.Create(
        Severity.Error, "Expected integer value for pipeline state {0} instead got {1}");

    internal static Diagnostic DiagExpectedIntegerValue(ISourceFile source, TextRange range, string context, string token) =>
        TypeExpectedIntegerValue.Create([context, token], [new(source, range)]);

    internal static readonly DiagnosticType TypeIntegerValueTooLarge = CategoryLanguage.Create(
        Severity.Error, "Expected integer value between 0 and {1} for {0}");

    internal static Diagnostic DiagIntegerValueTooLarge(ISourceFile source, TextRange range, string context, ulong maxValue) =>
        TypeIntegerValueTooLarge.Create([context, maxValue], [new(source, range)]);

    internal static readonly DiagnosticType TypeUnknownBlendFactor = CategoryLanguage.Create(
        Severity.Error, "Unknown blend factor: {0}");

    internal static Diagnostic DiagUnknownBlendFactor(ISourceFile source, Tk token) =>
        TypeUnknownBlendFactor.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownBlendFunction = CategoryLanguage.Create(
        Severity.Error, "Unknown blend function: {0}");

    internal static Diagnostic DiagUnknownBlendFunction(ISourceFile source, TextRange range, string token) =>
        TypeUnknownBlendFunction.Create([token], [new(source, range)]);

    internal static readonly DiagnosticType TypeUnknownComparisonKind = CategoryLanguage.Create(
        Severity.Error, "Unknown comparison kind: {0}");

    internal static Diagnostic DiagUnknownComparisonKind(ISourceFile source, TextRange range, string token) =>
        TypeUnknownComparisonKind.Create([token], [new(source, range)]);

    internal static readonly DiagnosticType TypeUnknownStencilOperation = CategoryLanguage.Create(
        Severity.Error, "Unknown stencil operation: {0}");

    internal static Diagnostic DiagUnknownStencilOperation(ISourceFile source, Tk token) =>
        TypeUnknownStencilOperation.Create([token.Text], [new(source, token.Range)]);


    internal static readonly DiagnosticType TypeUnknownFaceCullMode = CategoryLanguage.Create(
        Severity.Error, "Unknown face cull mode: {0}");

    internal static Diagnostic DiagUnknownFaceCullMode(ISourceFile source, Tk token) =>
        TypeUnknownFaceCullMode.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownPolygonFillMode = CategoryLanguage.Create(
        Severity.Error, "Unknown polygon fill mode: {0}");

    internal static Diagnostic DiagUnknownPolygonFillMode(ISourceFile source, Tk token) =>
        TypeUnknownPolygonFillMode.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownFrontFace = CategoryLanguage.Create(
        Severity.Error, "Unknown front face: {0}");

    internal static Diagnostic DiagUnknownFrontFace(ISourceFile source, Tk token) =>
        TypeUnknownFrontFace.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownPrimitiveTopology = CategoryLanguage.Create(
        Severity.Error, "Unknown primitive topology: {0}");

    internal static Diagnostic DiagUnknownPrimitiveTopology(ISourceFile source, Tk token) =>
        TypeUnknownPrimitiveTopology.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownPixelFormat = CategoryLanguage.Create(
        Severity.Error, "Unknown pixel format: {0}");

    internal static Diagnostic DiagUnknownPixelFormat(ISourceFile source, Tk token) =>
        TypeUnknownPixelFormat.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeVariantSpaceTooLarge = CategoryLanguage.Create(
        Severity.Error, "Variant space is too large, it exceeds {0} bits. Maximum is {1} bits");

    internal static Diagnostic DiagVariantSpaceTooLarge(int curBits, int maxBits) =>
        TypeVariantSpaceTooLarge.Create([curBits, maxBits]);

    internal static readonly DiagnosticType TypeOptionConditionNotEvaluable = CategoryLanguage.Create(
        Severity.Error, "Global block condition is not evaluable");

    internal static Diagnostic DiagOptionConditionNotEvaluable(ISourceFile source, TextRange range) =>
        TypeOptionConditionNotEvaluable.Create(sourceInfos: [new(source, range)]);

    internal static readonly DiagnosticType TypeOptionConditionIsConstant = CategoryLanguage.Create(
        Severity.Warning, "Global block condition is constant and always evaluates to {0}");

    internal static Diagnostic DiagOptionConditionIsConstant(ISourceFile source, TextRange range, long value) =>
        TypeOptionConditionIsConstant.Create([value == 0 ? "false" : "true"], [new(source, range)]);

    internal static readonly DiagnosticType TypeDuplicateOptionName = CategoryLanguage.Create(
        Severity.Error, "Option name {0} is duplicated", "Previous declaration", "New declaration");

    internal static Diagnostic DiagDuplicateOptionName(ISourceFile source, ASTOption prevOption, ASTOption newOption) =>
        TypeDuplicateOptionName.Create([prevOption.Name], [new(source, prevOption.Range), new(source, newOption.Range)]);

    internal static readonly DiagnosticType TypeOptionNameIsValue = CategoryLanguage.Create(
        Severity.Error, "Named option value {0} is also an option name", "Option declaration", "Named option value declaration");

    internal static Diagnostic DiagOptionNameIsValue(ISourceFile source, ASTOption nameOption, ASTOption valueOption, string valueName) =>
        TypeOptionNameIsValue.Create([valueName], [new(source, nameOption.Range), new(source, valueOption.Range)]);

    internal static readonly DiagnosticType TypeDuplicateNamedValue = CategoryLanguage.Create(
        Severity.Error, "Named option value {0} is declared twice with different values", "Previous declaration", "New declaration");

    internal static Diagnostic DiagDuplicateNamedValue(ISourceFile source, ASTOption prevOption, ASTOption newOption, string valueName) =>
        TypeDuplicateNamedValue.Create([valueName], [new(source, prevOption.Range), new(source, newOption.Range)]);

    internal static readonly DiagnosticType TypeSpecialOptionWithValues = CategoryLanguage.Create(
        Severity.Error, "Special option {0} should not have named values");

    internal static Diagnostic DiagSpecialOptionWithValues(ISourceFile source, ASTOption option) =>
        TypeSpecialOptionWithValues.Create([option.Name], [new(source, option.Range)]);

    internal static readonly DiagnosticType TypeInstancedBlockWithoutOption = CategoryLanguage.Create(
        Severity.Warning, "Instance storage block is used without the special IsInstanced option");

    internal static Diagnostic DiagInstancesBlockWithoutOption(ISourceFile source, TextRange range) =>
        TypeInstancedBlockWithoutOption.Create(sourceInfos: [new(source, range)]);

    internal static readonly DiagnosticType TypeNoStageBlock = CategoryLanguage.Create(
        Severity.Error, "Could not find applicable stage block for {0}");

    internal static Diagnostic DiagNoStageBlock(TokenKind stageKind) =>
        TypeNoStageBlock.Create([stageKind]);

    internal static readonly DiagnosticType TypeMultipleStageBlocks = CategoryLanguage.Create(
        Severity.Error, "Found multiple stage blocks for {0}");

    internal static Diagnostic DiagMultipleStageBlocks(ISourceFile source, TextRange range1, TextRange range2, TokenKind stageKind) =>
        TypeMultipleStageBlocks.Create([stageKind], [new(source, range1), new(source, range2)]);

    internal static readonly DiagnosticType TypeDuplicateStorageName = CategoryLanguage.Create(
        Severity.Error, "Found duplicated and applicable storage variable name {0}");

    internal static Diagnostic DiagDuplicateStorageName(ISourceFile source, ASTDeclaration prevDecl, ASTDeclaration newDecl) =>
        TypeDuplicateStorageName.Create([prevDecl.Name], [new(source, prevDecl.Range), new(source, newDecl.Range)]);
}
