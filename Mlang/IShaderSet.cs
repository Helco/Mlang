using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Mlang.Model;

namespace Mlang;

public interface IShaderSet : IDisposable
{
    public bool TryGetShaderInfo(string name, [NotNullWhen(true)] out ShaderInfo? shaderInfo);
    public bool TryGetSource(uint shaderHash, out string source);
    public bool TryGetVariant(ShaderVariantKey key, [NotNullWhen(true)] out ShaderVariant? variant);
}

// for .NET Standard 2.0 compatibility we have to use extension methods

public static class ShaderSetExtensions
{
    public static ShaderInfo GetShaderInfo(this IShaderSet set, string name) =>
        set.TryGetShaderInfo(name, out var info) ? info
        : throw new KeyNotFoundException($"ShaderSet does not contain shader named: {name}");

    public static string GetSource(this IShaderSet set, uint shaderHash) =>
        set.TryGetSource(shaderHash, out var source) ? source
        : throw new KeyNotFoundException($"ShaderSet does not contain source for shader hash {shaderHash:X8}");

    public static ShaderVariant GetVariant(this IShaderSet set, ShaderVariantKey key) =>
        set.TryGetVariant(key, out var variant) ? variant
        : throw new KeyNotFoundException($"ShaderSet does not contain variant for shader variant {key}");

    public static bool TryGetSource(this IShaderSet set, string name, out string source)
    {
        source = "";
        return set.TryGetShaderInfo(name, out var shaderInfo) &&
            set.TryGetSource(shaderInfo.SourceHash, out source);
    }

    public static string GetSource(this IShaderSet set, string name) =>
        set.TryGetSource(name, out var source) ? source
        : throw new KeyNotFoundException($"ShaderSet does not contain source for shader \"{name}\"");
}
