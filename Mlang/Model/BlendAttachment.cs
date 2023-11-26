using System;

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
);

public readonly record struct BlendAttachment(
    BlendFormula Color,
    BlendFormula? Alpha
);
