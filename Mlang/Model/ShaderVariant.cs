using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Mlang.Model;

public interface IShaderVariantKeyable
{
    ShaderVariantKey VariantKey { get; }
}

public readonly record struct ShaderVariantKey(
    uint ShaderHash,
    uint OptionBits,
    uint CompilerHash)
    : IShaderVariantKeyable
{
    public ShaderVariantKey VariantKey => this;
    public override string ToString() => $"{ShaderHash:X8}_{OptionBits:X8}_{CompilerHash:X8}";
}

public class ShaderVariant : IShaderVariantKeyable
{
    private readonly byte[] vertexShader;
    private readonly byte[] fragmentShader;

    public ShaderVariantKey VariantKey { get; }
    public PipelineState PipelineState { get; }
    public ReadOnlySpan<byte> VertexShader => vertexShader;
    public ReadOnlySpan<byte> FragmentShader => fragmentShader;

    internal ShaderVariant(
        ShaderVariantKey variantKey,
        PipelineState pipelineState,
        byte[] vertexShader,
        byte[] fragmentShader)
    {
        VariantKey = variantKey;
        PipelineState = pipelineState;
        this.vertexShader = vertexShader;
        this.fragmentShader = fragmentShader;
    }
}
