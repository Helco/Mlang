using System;
using System.Collections.Generic;

namespace Mlang.Model;

public interface IShaderVariantKeyable
{
    ShaderVariantKey VariantKey { get; }
}

public readonly record struct ShaderVariantKey(
    uint ShaderHash,
    uint OptionBits)
    : IShaderVariantKeyable
{
    public ShaderVariantKey VariantKey => this;
    public override string ToString() => $"{ShaderHash:X8}_{OptionBits:X8}";
}

public class ShaderVariant : IShaderVariantKeyable
{
    private readonly byte[] vertexShader;
    private readonly byte[] fragmentShader;

    public ShaderVariantKey VariantKey { get; }
    public PipelineState PipelineState { get; }
    public IReadOnlyList<VertexAttributeInfo> VertexAttributes { get; }
    public IReadOnlyList<int> BindingSetSizes { get; }
    /// <remarks>Sorted by set and binding indices</remarks>
    public IReadOnlyList<IBindingInfo> Bindings { get; }
    public ReadOnlySpan<byte> VertexShader => vertexShader;
    public ReadOnlySpan<byte> FragmentShader => fragmentShader;

    public ShaderVariant(
        ShaderVariantKey variantKey,
        PipelineState pipelineState,
        IReadOnlyList<VertexAttributeInfo> vertexAttributes,
        IReadOnlyList<int> bindingSetSizes,
        IReadOnlyList<IBindingInfo> bindings,
        byte[] vertexShader,
        byte[] fragmentShader)
    {
        this.vertexShader = vertexShader;
        this.fragmentShader = fragmentShader;
        VariantKey = variantKey;
        PipelineState = pipelineState;
        VertexAttributes = vertexAttributes;
        BindingSetSizes = bindingSetSizes;
        Bindings = bindings;
    }
}
