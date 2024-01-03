using System;
using System.IO;

namespace Mlang.Model;

public enum BlendFactor
{
    BlendFactor,
    DstAlpha,
    DstColor,
    InvBlendFactor,
    InvDstAlpha,
    InvDstColor,
    InvSrcAlpha,
    InvSrcColor,
    One,
    SrcAlpha,
    SrcColor,
    Zero
}

public enum BlendFunction
{
    Add,
    Maximum,
    Minimum,
    ReverseSubtract,
    Subtract
}

public readonly record struct BlendFormula(
    BlendFactor Source,
    BlendFactor Destination,
    BlendFunction Function
)
{
    internal static BlendFormula Read(BinaryReader reader) => new(
        (BlendFactor)reader.ReadByte(),
        (BlendFactor)reader.ReadByte(),
        (BlendFunction)reader.ReadByte());

    internal void Write(BinaryWriter writer)
    {
        writer.Write((byte)Source);
        writer.Write((byte)Destination);
        writer.Write((byte)Function);
    }
}

public readonly record struct BlendAttachment(
    BlendFormula? Color,
    BlendFormula? Alpha
)
{
    public static readonly BlendAttachment NoBlend = default;

    internal static BlendAttachment Read(BinaryReader reader)
    {
        var isNoBlend = reader.ReadBoolean();
        if (isNoBlend)
            return NoBlend;
        var hasAlpha = reader.ReadBoolean();
        return new(BlendFormula.Read(reader), hasAlpha ? BlendFormula.Read(reader) : null);
    }

    internal static void Write(BinaryWriter writer, BlendAttachment a)
    {
        if (a.Color == null)
        {
            writer.Write(false);
            return;
        }
        writer.Write(a.Alpha.HasValue);
        a.Color.Value.Write(writer);
        a.Alpha?.Write(writer);
    }
}
