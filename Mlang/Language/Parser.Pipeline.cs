using System;
using Mlang.Model;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang.Language;

partial class Parser
{
    private BlendFactor ParseBlendFactor(Tk factor)
    {
        if (!Enum.TryParse<BlendFactor>(factor.Text, ignoreCase: true, out var result))
            diagnostics.Add(Mlang.Diagnostics.DiagUnknownBlendFactor(SourceFile, factor));
        return result;
    }

    private BlendFunction ParseBlendFunction(Tk token)
    {
        switch (token.Kind)
        {
            case TokenKind.Identifier:
                if (!Enum.TryParse<BlendFunction>(token.Text, ignoreCase: true, out var result))
                    diagnostics.Add(Mlang.Diagnostics.DiagUnknownBlendFunction(SourceFile, token.Range, token.Text));
                return result;
            case TokenKind.Add: return BlendFunction.Add;
            case TokenKind.Subtract: return BlendFunction.Subtract;
            case TokenKind.BitNegate: return BlendFunction.ReverseSubtract;
            case TokenKind.Greater: return BlendFunction.Maximum;
            case TokenKind.Lesser: return BlendFunction.Minimum;
            default:
                diagnostics.Add(Mlang.Diagnostics.DiagUnknownBlendFunction(SourceFile, token.Range, token.Kind.ToString()));
                return BlendFunction.Add;
        }
    }
}
