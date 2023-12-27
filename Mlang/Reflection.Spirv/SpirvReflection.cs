using System;
using System.Collections.Generic;
using Mlang.Model;

namespace Mlang.Reflection.Spirv;

internal class SpirvReflection : IReflectionSource
{
    private readonly List<Diagnostic> diagnostics = new();
    private readonly ShaderInfo shaderInfo;
    private readonly SpirvModuleReflection vertex;
    private readonly SpirvModuleReflection fragment;

    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;

    public SpirvReflection(ShaderInfo shaderInfo, ShaderVariant variant)
    {
        this.shaderInfo = shaderInfo;
        vertex = new(variant.VertexShader);
        fragment = new(variant.FragmentShader);
    }

    public ShaderVariantInfo ReflectVariant()
    {
        var variantInfo = new ShaderVariantInfo()
        {
            
        }
    }
}
