﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Mlang.Model;

namespace Mlang;

public interface IShaderSet
{
    public bool TryGetShaderInfo(string name, [NotNullWhen(true)] out ShaderInfo? shaderInfo);
    public bool TryGetSource(uint shaderHash, out ReadOnlySpan<byte> source);
    public bool TryGetVariant(ShaderVariantKey key, [NotNullWhen(true)] out ShaderVariant? variant);
}

// for .NET Standard 2.0 compatibility we have to use extension methods

public static class ShaderSetExtensions
{
    public static ShaderInfo GetShaderInfo(this IShaderSet set, string name) =>
        set.TryGetShaderInfo(name, out var info) ? info
        : throw new KeyNotFoundException($"ShaderSet does not contain shader named: {name}");

    public static ReadOnlySpan<byte> GetSource(this IShaderSet set, uint shaderHash) =>
        set.TryGetSource(shaderHash, out var source) ? source
        : throw new KeyNotFoundException($"ShaderSet does not contain source for shader hash {shaderHash:X8}");

    public static ShaderVariant GetVariant(this IShaderSet set, ShaderVariantKey key) =>
        set.TryGetVariant(key, out var variant) ? variant
        : throw new KeyNotFoundException($"ShaderSet does not contain variant for shader variant {key}");
}
