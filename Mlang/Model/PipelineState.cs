using System;
using System.Collections.Generic;
using System.IO;
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

    public static PartialStencilState AsDifference(StencilState target, StencilState from)
    {
        var state = new PartialStencilState();
        state.Comparison = AsDifference(target.Comparison, from.Comparison);
        state.Pass = AsDifference(target.Pass, from.Pass);
        state.Fail = AsDifference(target.Fail, from.Fail);
        state.DepthFail = AsDifference(target.DepthFail, from.DepthFail);
        return state;
    }

    private static T? AsDifference<T>(T target, T from) where T : struct =>
        EqualityComparer<T>.Default.Equals(target, from) ? null : target;
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
    public FaceFillMode? FillMode;
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

    /// <summary>Returns a partial state P such that <code>from.With(P).Equals(to)</code></summary>
    public static PartialPipelineState AsDifference(PipelineState to, PipelineState from)
    {
        var state = new PartialPipelineState();
        state.CoverageToAlpha = AsDifference(to.CoverageToAlpha, from.CoverageToAlpha);
        state.BlendFactor = AsDifference(to.BlendFactor, from.BlendFactor);
        state.BlendAttachments = to.BlendAttachments.SequenceEqual(from.BlendAttachments)
            ? Array.Empty<BlendAttachment>()
            : to.BlendAttachments.ToArray();

        state.DepthTest = AsDifference(to.DepthTest, from.DepthTest);
        state.DepthWrite = AsDifference(to.DepthWrite, from.DepthWrite);
        state.StencilTest = AsDifference(to.StencilTest, from.StencilTest);
        state.StencilReadMask = AsDifference(to.StencilReadMask, from.StencilReadMask);
        state.StencilWriteMask = AsDifference(to.StencilWriteMask, from.StencilWriteMask);
        state.StencilReference = AsDifference(to.StencilReference, from.StencilReference);
        state.Stencil = default;
        state.StencilBack = PartialStencilState.AsDifference(to.StencilBack, from.StencilBack);
        state.StencilFront = PartialStencilState.AsDifference(to.StencilFront, from.StencilFront);

        state.CullMode = AsDifference(to.CullMode, from.CullMode);
        state.FillMode = AsDifference(to.FillMode, from.FillMode);
        state.FrontFace = AsDifference(to.FrontFace, from.FrontFace);
        state.DepthClip = AsDifference(to.DepthClip, from.DepthClip);
        state.ScissorTest = AsDifference(to.ScissorTest, from.ScissorTest);

        state.PrimitiveTopology = AsDifference(to.PrimitiveTopology, from.PrimitiveTopology);
        state.DepthOutput = to.DepthOutput;
        state.ColorOutputs =
            to.ColorOutputs.Count == from.ColorOutputs.Count &&
            to.ColorOutputs.Zip(from.ColorOutputs, (a, b) => a.Key == b.Key && a.Value == b.Value).All(b => b)
            ? Array.Empty<KeyValuePair<string, PixelFormat>>()
            : to.ColorOutputs.ToArray();
        state.OutputSamples = AsDifference(to.OutputSamples, from.OutputSamples);
        return state;
    }

    private static T? AsDifference<T>(T target, T from) where T : struct =>
        EqualityComparer<T>.Default.Equals(target, from) ? null : target;
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
    public FaceFillMode FillMode { get; init; }
    public FrontFace FrontFace { get; init; }
    public bool DepthClip { get; init; }
    public bool ScissorTest { get; init; }

    public PrimitiveTopology PrimitiveTopology { get; init; }

    public PixelFormat? DepthOutput { get; init; }
    public required IReadOnlyList<KeyValuePair<string, PixelFormat>> ColorOutputs { get; init; }
    public byte OutputSamples { get; init; }

    public static readonly PipelineState Default = GetDefault(1);
    public static PipelineState GetDefault(int attachmentCounts) => new()
    {
        BlendAttachments = new BlendAttachment[attachmentCounts],
        ColorOutputs = new KeyValuePair<string, PixelFormat>[attachmentCounts],
        DepthTest = true,
        DepthWrite = true,
        CullMode = FaceCullMode.Back,
        FillMode = FaceFillMode.Solid,
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

    internal static PipelineState Read(BinaryReader reader) => new()
    {
        CoverageToAlpha = reader.ReadBoolean(),
        BlendFactor = reader.ReadVector4(),
        BlendAttachments = reader.ReadArray(BlendAttachment.Read),

        DepthTest = reader.ReadBoolean(),
        DepthWrite = reader.ReadBoolean(),
        StencilTest = reader.ReadBoolean(),
        StencilReadMask = reader.ReadByte(),
        StencilWriteMask = reader.ReadByte(),
        StencilReference = reader.ReadUInt32(),
        StencilFront = StencilState.Read(reader),
        StencilBack = StencilState.Read(reader),

        CullMode = (FaceCullMode)reader.ReadByte(),
        FillMode = (FaceFillMode)reader.ReadByte(),
        FrontFace = (FrontFace)reader.ReadByte(),
        DepthClip = reader.ReadBoolean(),
        ScissorTest = reader.ReadBoolean(),

        PrimitiveTopology = (PrimitiveTopology)reader.ReadByte(),
        DepthOutput = reader.ReadNullable(ReadPixelFormat),
        ColorOutputs = reader.ReadArray(ReadColorOutput),
        OutputSamples = reader.ReadByte()
    };

    internal void Write(BinaryWriter writer)
    {
        writer.Write(CoverageToAlpha);
        writer.Write(BlendFactor);
        writer.Write(BlendAttachments, BlendAttachment.Write);

        writer.Write(DepthTest);
        writer.Write(DepthWrite);
        writer.Write(StencilTest);
        writer.Write(StencilReadMask);
        writer.Write(StencilWriteMask);
        writer.Write(StencilReference);
        StencilFront.Write(writer);
        StencilBack.Write(writer);

        writer.Write((byte)CullMode);
        writer.Write((byte)FillMode);
        writer.Write((byte)FrontFace);
        writer.Write(DepthClip);
        writer.Write(ScissorTest);

        writer.Write((byte)PrimitiveTopology);

        writer.Write(DepthOutput, WritePixelFormat);
        writer.Write(ColorOutputs, WriteColorOutput);
        writer.Write(OutputSamples);
    }

    private static PixelFormat ReadPixelFormat(BinaryReader reader) => (PixelFormat)reader.ReadByte();
    private static void WritePixelFormat(BinaryWriter writer, PixelFormat format) => writer.Write((byte)format);
    private static KeyValuePair<string, PixelFormat> ReadColorOutput(BinaryReader reader) => new(
        reader.ReadString(),
        (PixelFormat)reader.ReadByte());
    private static void WriteColorOutput(BinaryWriter writer, KeyValuePair<string, PixelFormat> pair)
    {
        writer.Write(pair.Key);
        writer.Write((byte)pair.Value);
    }
}
