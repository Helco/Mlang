using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
namespace Mlang.Model;

internal struct PartialStencilState
{
    public ComparisonKind? Comparison;
    public StencilOperation? Pass;
    public StencilOperation? Fail;
    public StencilOperation? DepthFail;
}

internal class PartialPipelineState
{
    public bool? CoverageToAlpha;
    public Vector4? BlendFactor;
    public BlendAttachment[] BlendAttachments = Array.Empty<BlendAttachment>();

    public bool? DepthTest;
    public bool? DepthWrite;
    public bool? StencilTest;
    public byte? StencilReadMask;
    public byte? StencilWriteMask;
    public uint? StencilReference;
    public PartialStencilState Stencil;
    public PartialStencilState StencilFront;
    public PartialStencilState StencilBack;

    public FaceCullMode? CullMode;
    public PolygonFillMode? FillMode;
    public FrontFace? FrontFace;
    public bool? DepthClip;
    public bool? ScissorTest;

    public PrimitiveTopology? PrimitiveTopology;
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
}

public record PipelineState
{
    public bool CoverageToAlpha { get; init; }
    public Vector4 BlendFactor { get; init; }
    public required IReadOnlyList<BlendAttachment> BlendAttachments { get; init; }

    public bool DepthTest { get; init; }
    public bool DepthWrite { get; init; }
    public bool StencilTest { get; init; }
    public byte StencilReadMask { get; init; }
    public byte StencilWriteMask { get; init; }
    public uint StencilReference { get; init; }
    public StencilState StencilFront { get; init; }
    public StencilState StencilBack { get; init; }

    public FaceCullMode CullMode { get; init; }
    public PolygonFillMode FillMode { get; init; }
    public FrontFace FrontFace { get; init; }
    public bool DepthClip { get; init; }
    public bool ScissorTest { get; init; }

    public PrimitiveTopology PrimitiveTopology { get; init; }

    public static PipelineState GetDefault(int attachmentCounts) => new()
    {
        BlendAttachments = new BlendAttachment[attachmentCounts],
        DepthTest = true,
        DepthWrite = true,
        CullMode = FaceCullMode.Back,
        FillMode = PolygonFillMode.Solid,
        FrontFace = FrontFace.CounterClockwise,
        DepthClip = true,
        PrimitiveTopology = PrimitiveTopology.TriangleList
    };

    internal PipelineState With(PartialPipelineState s) => new()
    {
        CoverageToAlpha = s.CoverageToAlpha ?? CoverageToAlpha,
        BlendFactor = s.BlendFactor ?? BlendFactor,
        BlendAttachments =
            s.BlendAttachments.Concat(BlendAttachments)
            .Take(Math.Max(s.BlendAttachments.Length, BlendAttachments.Count))
            .ToArray(),

        DepthTest = s.DepthTest ?? DepthTest,
        DepthWrite = s.DepthWrite ?? DepthWrite,
        StencilTest = s.StencilTest ?? StencilTest,
        StencilReadMask = s.StencilReadMask ?? StencilReadMask,
        StencilWriteMask = s.StencilWriteMask ?? StencilWriteMask,
        StencilReference = s.StencilReference ?? StencilReference,
        StencilFront = StencilFront.With(s.StencilFront, s.Stencil),
        StencilBack = StencilBack.With(s.StencilBack, s.Stencil),

        CullMode = s.CullMode ?? CullMode,
        FillMode = s.FillMode ?? FillMode,
        FrontFace = s.FrontFace ?? FrontFace,
        DepthClip = s.DepthClip ?? DepthClip,
        ScissorTest = s.ScissorTest ?? ScissorTest,

        PrimitiveTopology = s.PrimitiveTopology ?? PrimitiveTopology
    };
}
