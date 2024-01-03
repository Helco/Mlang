using System.IO;
namespace Mlang.Model;

public enum ComparisonKind
{
    Always,
    Equal,
    Greater,
    GreaterEqual,
    Less,
    LessEqual,
    Never,
    NotEqual
}

public enum StencilOperation
{
    DecrementAndClamp,
    DecrementAndWrap,
    IncrementAndClamp,
    IncrementAndWrap,
    Invert,
    Keep,
    Replace,
    Zero
}

public readonly record struct StencilState(
    ComparisonKind Comparison,
    StencilOperation Pass,
    StencilOperation Fail,
    StencilOperation DepthFail)
{
    internal StencilState With(in PartialStencilState primary, in PartialStencilState secondary) => new(
        primary.Comparison ?? secondary.Comparison ?? Comparison,
        primary.Pass ?? secondary.Pass ?? Pass,
        primary.Fail ?? secondary.Fail ?? Fail,
        primary.DepthFail ?? secondary.DepthFail ?? DepthFail);

    internal static StencilState Read(BinaryReader reader) => new(
        (ComparisonKind)reader.ReadByte(),
        (StencilOperation)reader.ReadByte(),
        (StencilOperation)reader.ReadByte(),
        (StencilOperation)reader.ReadByte());

    internal void Write(BinaryWriter writer)
    {
        writer.Write((byte)Comparison);
        writer.Write((byte)Pass);
        writer.Write((byte)Fail);
        writer.Write((byte)DepthFail);
    }
}
