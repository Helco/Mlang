﻿using System;
using System.Collections.Generic;
using System.IO;
using Silk.NET.Core;

namespace Mlang.Model;

public record OptionInfo(string name, string[]? namedValues = null)
{
    public string Name => name;
    public string[]? NamedValues => namedValues;

    internal int BitCount => NamedValues == null ? 1 :
        (int)Math.Ceiling(Math.Log(NamedValues.Length) / Math.Log(2));
}

public record VertexAttributeInfo
{
    public int AttributeIndex { get; init; }
    public required string Name { get; init; }
    public required NumericType Type { get; init; }
    public bool IsInstance { get; init; }

    internal static void Write(BinaryWriter writer, VertexAttributeInfo info)
    {
        writer.Write(info.AttributeIndex);
        writer.Write(info.Name);
        writer.Write(info.IsInstance);
        info.Type.Write(writer);
    }

    internal static VertexAttributeInfo Read(BinaryReader reader) => new()
    {
        AttributeIndex = reader.ReadInt32(),
        Name = reader.ReadString(),
        IsInstance = reader.ReadBoolean(),
        Type = NumericType.Read(reader)
    };
}

public record ShaderInfo
{
    public required uint SourceHash { get; init; }
    public required IReadOnlyList<OptionInfo> Options { get; init; }
    public required IReadOnlyList<string> VertexAttributes { get; init; }
    public required IReadOnlyList<string> InstanceAttributes { get; init; }
    public required IReadOnlyList<string> Bindings { get; init; }

    public ShaderVariantKey VariantKeyFor(IReadOnlyDictionary<string, uint> options)
    {
        uint variantBits = 0;
        int curBitOffset = 0;
        foreach (var option in Options)
        {
            if (!options.TryGetValue(option.Name, out var value) ||
                value >= (option.NamedValues?.Length ?? 2))
                value = 0;
            variantBits |= (value << curBitOffset);
            curBitOffset += option.BitCount;
        }
        return new(SourceHash, variantBits);
    }
}

public record ShaderVariantInfo
{
    /// <remarks>Sorted by attribute index</remarks>
    public required IReadOnlyList<VertexAttributeInfo> VertexAttributes { get; init; }
    public required IReadOnlyList<int[]> BindingSetSizes { get; init; }
    /// <remarks>Sorted by set and binding indices</remarks>
    public required IReadOnlyList<IBindingInfo> Bindings { get; init; }
}