using System;

namespace Mlang.Model;

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
