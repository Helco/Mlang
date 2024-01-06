using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Lexer.Attributes;

namespace Mlang.Language;

internal enum TokenKind
{
    [Error] Error,
    [End] End,
    [Ignore] [Regex(Regexes.Whitespace)] Ignore,
    [Ignore]
    [Regex(Regexes.Whitespace)]
    [Regex(Regexes.LineComment)]
    [Regex(Regexes.MultilineComment)] Ignored,

#region Punctuation
    [Token("{")] ScopeBrL,
    [Token("}")] ScopeBrR,
    [Token("(")] ExprBrL,
    [Token(")")] ExprBrR,
    [Token("[")] ArrayBrL,
    [Token("]")] ArrayBrR,

    [Token("&&")] LogicalAnd,
    [Token("||")] LogicalOr,
    [Token("^^")] LogicalXor,
    [Token("++")] Increment,
    [Token("--")] Decrement,
    [Token("<<")] BitshiftL,
    [Token(">>")] BitshiftR,
    [Token("+")] Add,
    [Token("-")] Subtract,
    [Token("*")] Multiplicate,
    [Token("/")] Divide,
    [Token("%")] Modulo,
    [Token("^")] BitXor,
    [Token("&")] BitAnd,
    [Token("|")] BitOr,
    [Token("~")] BitNegate,
    [Token(".")] Dot,
    [Token(",")] Comma,
    [Token(":")] Colon,
    [Token(";")] Semicolon,
    [Token("?")] Question,
    [Token("!")] Ampersand,
    [Token("+=")] AddAssign,
    [Token("-=")] SubtractAssign,
    [Token("*=")] MultiplicateAssign,
    [Token("/=")] DivideAssign,
    [Token("%=")] ModuloAssign,
    [Token("&=")] AndAssign,
    [Token("|=")] OrAssign,
    [Token("^=")] XorAssign,
    [Token("<=")] LessOrEquals,
    [Token(">=")] GreaterOrEquals,
    [Token("==")] Equals,
    [Token("!=")] NotEquals,
    [Token("<")] Lesser,
    [Token(">")] Greater,
    [Token("=")] Assign,
#endregion // Punctuation

#region Flow control
    [Token("break")] KwBreak,
    [Token("continue")] KwContinue,
    [Token("do")] KwDo,
    [Token("else")] KwElse,
    [Token("for")] KwFor,
    [Token("if")] KwIf,
    [Token("discard")] KwDiscard,
    [Token("return")] KwReturn,
    [Token("switch")] KwSwitch,
    [Token("case")] KwCase,
    [Token("default")] KwDefault,
    [Token("while")] KwWhile,
#endregion

#region Mlang specific keywords
    [Token("option")] KwOption,
    [Token("attributes")] KwAttributes,
    [Token("instances")] KwInstances,
    [Token("varying")] KwVarying,
    [Token("uniform")] KwUniform,
    [Token("pipeline")] KwPipeline,
    [Token("vertex")] KwVertex,
    [Token("fragment")] KwFragment,
    [Token("blend")] KwBlend,
    [Token("buffer")] KwBuffer,
#endregion

    [Regex(Regexes.Identifier)] Identifier,
    [Regex(Regexes.HexLiteral)]
    [Regex(Regexes.IntLiteral)] UnsignedInteger,
    [Regex(Regexes.RealNumberLiteral + "f?")] UnsignedReal,
}

[Lexer(typeof(TokenKind))]
internal partial class Lexer { }
