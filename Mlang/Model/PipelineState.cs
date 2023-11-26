using System;
using System.Collections.Generic;
using System.Numerics;

namespace Mlang.Model;

public record PipelineState
{
    public bool? CoverageToAlpha { get; set; }
    public Vector4? BlendFactor { get; set; }
    public BlendAttachment[] BlendAttachments { get; set; } = Array.Empty<BlendAttachment>();
}
