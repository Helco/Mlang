using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Parser;
using Yoakke.SynKit.Reporting.Present;
using Yoakke.SynKit.Text;
using TextRange = Yoakke.SynKit.Text.Range;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang;

partial class Diagnostics
{
    internal static readonly DiagnosticCategory CategoryLanguage = new("LANG");

    internal static readonly DiagnosticType TypeLexer = CategoryLanguage.Error("Found invalid token");
    internal static Diagnostic DiagLexer(ISourceFile source, IToken<TokenKind> token) =>
        TypeLexer.Create(sourceInfos: [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeParser = CategoryLanguage.CreateWithFootNote(
        Severity.Error, "Could not parse {0} at this point", "{1}");
    internal static Diagnostic DiagParser(ISourceFile source, IToken<TokenKind> token, IEnumerable<ParseErrorElement> elements) =>
        TypeParser.Create(new object[]
        {
            token.Kind,
            string.Join("\n", elements.Select(e => $"For {e.Context} expected {string.Join(" or ", e.Expected)}"))
        }, new[] { new Location(source, token.Range) });

    internal static readonly DiagnosticType TypeTooFewOptions = CategoryLanguage.Create(
        Severity.Error, "An option needs at least two values");

    internal static Diagnostic DiagTooFewOptions(ISourceFile source, TextRange range) =>
        TypeTooFewOptions.Create(sourceInfos: [new(source, range)]);

    internal static readonly DiagnosticType TypeUnknownBlendFactor = CategoryLanguage.Create(
        Severity.Error, "Unknown blend factor: {0}");

    internal static Diagnostic DiagUnknownBlendFactor(ISourceFile source, Tk token) =>
        TypeUnknownBlendFactor.Create([token.Text], [new(source, token.Range)]);

    internal static readonly DiagnosticType TypeUnknownBlendFunction = CategoryLanguage.Create(
        Severity.Error, "Unknown blend function: {0}");
    
    internal static Diagnostic DiagUnknownBlendFunction(ISourceFile source, TextRange range, string token) =>
        TypeUnknownBlendFunction.Create([token], [new(source, range)]);
}
