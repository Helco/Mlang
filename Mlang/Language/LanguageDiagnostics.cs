using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Parser;
using Yoakke.SynKit.Text;
using TextRange = Yoakke.SynKit.Text.Range;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang;

partial class Diagnostics
{
    internal static readonly DiagnosticCategory CategoryLanguage = new("LANG");

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
}
