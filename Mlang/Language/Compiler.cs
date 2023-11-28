using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Reporting;
using Yoakke.SynKit.Text;

using static Mlang.Diagnostics;

namespace Mlang.Language;

public class Compiler
{
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
            if (result.IsOk)
            {
                unit = result.Ok.Value;
                return true;
            }

            var errToken = (IToken<TokenKind>)result.Error.Got!;

            if (errToken.Kind == TokenKind.Error)
                diagnostics.Add(DiagLexer(sourceFile, errToken));
            else
                diagnostics.Add(DiagParser(sourceFile, errToken, result.Error.Elements.Values));
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
}
