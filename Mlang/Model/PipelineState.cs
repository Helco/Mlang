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

    public PartialStencilState With(PartialStencilState state) => new()
    {
        Comparison = state.Comparison ?? Comparison,
        Pass = state.Pass ?? Pass,
        Fail = state.Fail ?? Fail,
        DepthFail = state.DepthFail ?? DepthFail
    };
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

    public PixelFormat? DepthOutput;
    public KeyValuePair<string, PixelFormat>[] ColorOutputs = Array.Empty<KeyValuePair<string, PixelFormat>>();
    public byte? OutputSamples;

    public void With(PartialPipelineState state)
    {
        CoverageToAlpha = state.CoverageToAlpha ?? CoverageToAlpha;
        BlendFactor = state.BlendFactor ?? BlendFactor;
        BlendAttachments = BlendAttachments.Concat(state.BlendAttachments).ToArray();

        DepthTest = state.DepthTest ?? DepthTest;
        DepthWrite = state.DepthWrite ?? DepthWrite;
        StencilTest = state.StencilTest ?? StencilTest;
        StencilReadMask = state.StencilReadMask ?? StencilReadMask;
        StencilWriteMask = state.StencilWriteMask ?? StencilWriteMask;
        StencilReference = state.StencilReference ?? StencilReference;
        Stencil = Stencil.With(state.Stencil);
        StencilFront = StencilFront.With(state.StencilFront);
        StencilBack = StencilBack.With(state.StencilBack);

        CullMode = state.CullMode ?? CullMode;
        FillMode = state.FillMode ?? FillMode;
        FrontFace = state.FrontFace ?? FrontFace;
        DepthClip = state.DepthClip ?? DepthClip;
        ScissorTest = state.ScissorTest ?? ScissorTest;

        PrimitiveTopology = state.PrimitiveTopology ?? PrimitiveTopology;

        DepthOutput = state.DepthOutput ?? DepthOutput;
        ColorOutputs = ColorOutputs.Concat(state.ColorOutputs).ToArray();
        OutputSamples = state.OutputSamples ?? OutputSamples;
    }
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

    public PixelFormat? DepthOutput { get; init; }
    public required IReadOnlyList<KeyValuePair<string, PixelFormat>> ColorOutputs { get; init; }
    public byte OutputSamples { get; init; }

    public static PipelineState GetDefault(int attachmentCounts) => new()
    {
        BlendAttachments = new BlendAttachment[attachmentCounts],
        ColorOutputs = new KeyValuePair<string, PixelFormat>[attachmentCounts],
        DepthTest = true,
        DepthWrite = true,
        CullMode = FaceCullMode.Back,
        FillMode = PolygonFillMode.Solid,
        FrontFace = FrontFace.CounterClockwise,
        DepthClip = true,
        PrimitiveTopology = PrimitiveTopology.TriangleList,
        OutputSamples = 1
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

        PrimitiveTopology = s.PrimitiveTopology ?? PrimitiveTopology,

        DepthOutput = s.DepthOutput ?? DepthOutput,
        ColorOutputs =
            s.ColorOutputs.Concat(ColorOutputs)
            .Take(Math.Max(s.ColorOutputs.Length, ColorOutputs.Count))
            .ToArray(),
        OutputSamples = s.OutputSamples ?? OutputSamples
    };
}
