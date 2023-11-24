using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Parser;
using Yoakke.SynKit.Reporting.Present;
using Yoakke.SynKit.Text;

namespace Mlang;

partial class Diagnostics
{
    internal static readonly DiagnosticCategory CategoryLanguage = new("LANG");

    internal static readonly DiagnosticType TypeLexer = CategoryLanguage.Error("Found invalid token");
    internal static Diagnostic DiagLexer(ISourceFile source, IToken<TokenKind> token) =>
        TypeLexer.Create(sourceInfos: new[] { new Location(source, token.Range) });

    internal static readonly DiagnosticType TypeParser = CategoryLanguage.CreateWithFootNote(
        Severity.Error, "Could not parse {0} at this point", "{1}");
    internal static Diagnostic DiagParser(ISourceFile source, IToken<TokenKind> token, IEnumerable<ParseErrorElement> elements) =>
        TypeParser.Create(new object[]
        {
            token.Kind,
            string.Join("\n", elements.Select(e => $"For {e.Context} expected {string.Join(" or ", e.Expected)}"))
        }, new[] { new Location(source, token.Range) });
}
