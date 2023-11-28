using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Lexer.Attributes;

namespace Mlang.Language;

internal enum TokenKind
{
    [Error] Error,
    [End] End,
    [Ignore] [Regex(Regexes.Whitespace)] Ignore,
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

#region Numeric types
    [Token("void")] KwVoid,
    [Token("int")] [Token("int1")] KwInt1,
    [Token("int2")] [Token("ivec2")] KwInt2,
    [Token("int3")] [Token("ivec3")] KwInt3,
    [Token("int4")] [Token("ivec4")] KwInt4,
    [Token("uint")] [Token("uint1")] KwUint1,
    [Token("uint2")] [Token("uvec2")] KwUint2,
    [Token("uint3")] [Token("uvec3")] KwUint3,
    [Token("uint4")] [Token("uvec4")] KwUint4,
    [Token("float")] [Token("float1")] KwFloat1,
    [Token("float2")] [Token("vec2")] KwFloat2,
    [Token("float3")] [Token("vec3")] KwFloat3,
    [Token("float4")] [Token("vec4")] KwFloat4,

    [Token("byte2")] KwByte2,
    [Token("byte4")] KwByte4,
    [Token("sbyte2")] KwSbyte2,
    [Token("sbyte4")] KwSbyte4,
    [Token("short2")] KwShort2,
    [Token("short4")] KwShort4,
    [Token("ushort2")] KwUshort2,
    [Token("ushort4")] KwUshort4,
    [Token("byte2_norm")] KwByte2Norm,
    [Token("byte4_norm")] KwByte4Norm,
    [Token("sbyte2_norm")] KwSbyte2Norm,
    [Token("sbyte4_norm")] KwSbyte4Norm,
    [Token("short2_norm")] KwShort2Norm,
    [Token("short4_norm")] KwShort4Norm,
    [Token("ushort2_norm")] KwUshort2Norm,
    [Token("ushort4_norm")] KwUshort4Norm,
    [Token("half1")] KwHalf1,
    [Token("half2")] KwHalf2,
    [Token("half4")] KwHalf4,

    [Token("mat2")] [Token("mat2x2")] KwMat2x2,
    [Token("mat3x3")] KwMat2x3,
    [Token("mat3x4")] KwMat2x4,
    [Token("mat3x2")] KwMat3x2,
    [Token("mat3")] [Token("mat3x3")] KwMat3x3,
    [Token("mat3x4")] KwMat3x4,
    [Token("mat4x2")] KwMat4x2,
    [Token("mat4x3")] KwMat4x3,
    [Token("mat4")] [Token("mat4x4")] KwMat4x4,

#endregion // Numeric types

#region Image types
    [Token("sampler")] KwSampler,
    [Token("texture2D")] KwTexture2D,
    [Token("texture2DArray")] KwTexture2DArray,
    [Token("texture3D")] KwTexture3D,
    [Token("textureCube")] KwTextureCube,
    [Token("itexture2D")] KwITexture2D,
    [Token("itexture2DArray")] KwITexture2DArray,
    [Token("itexture3D")] KwITexture3D,
    [Token("itextureCube")] KwITextureCube,
    [Token("utexture2D")] KwUTexture2D,
    [Token("utexture2DArray")] KwUTexture2DArray,
    [Token("utexture3D")] KwUTexture3D,
    [Token("utextureCube")] KwUTextureCube,
#endregion

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
