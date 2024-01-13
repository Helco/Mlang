using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mlang.Model;

public record OptionInfo(string name, string[]? namedValues = null)
{
    public string Name => name;
    public string[]? NamedValues => namedValues;

    internal int BitCount => NamedValues == null ? 1 :
        (int)Math.Ceiling(Math.Log(NamedValues.Length) / Math.Log(2));

    internal static void Write(BinaryWriter writer, OptionInfo info)
    {
        writer.Write(info.name);
        writer.Write(info.NamedValues ?? Array.Empty<string>(), DotNetExtensions.Write);
    }

    internal static OptionInfo Read(BinaryReader reader)
    {
        var name = reader.ReadString();
        var namedValues = reader.ReadArray(DotNetExtensions.ReadString);
        return new(name, namedValues.None() ? null : namedValues);
    }
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
    /// <summary>A bitmask for the option bits resulting in clearing bits that do not change the shader programs</summary>
    public uint ProgramInvarianceMask { get; init; }
    public required IReadOnlyList<OptionInfo> Options { get; init; }
    public required IReadOnlyList<string> VertexAttributes { get; init; }
    public required IReadOnlyList<string> InstanceAttributes { get; init; }
    public required IReadOnlyList<string> Bindings { get; init; }

    internal void Write(BinaryWriter writer)
    {
        writer.Write(SourceHash);
        writer.Write(ProgramInvarianceMask);
        writer.Write(Options, OptionInfo.Write);
        writer.Write(VertexAttributes, DotNetExtensions.Write);
        writer.Write(InstanceAttributes, DotNetExtensions.Write);
        writer.Write(Bindings, DotNetExtensions.Write);
    }

    internal static ShaderInfo Read(BinaryReader reader) => new()
    {
        SourceHash = reader.ReadUInt32(),
        ProgramInvarianceMask = reader.ReadUInt32(),
        Options = reader.ReadArray(OptionInfo.Read),
        VertexAttributes = reader.ReadArray(DotNetExtensions.ReadString),
        InstanceAttributes = reader.ReadArray(DotNetExtensions.ReadString),
        Bindings = reader.ReadArray(DotNetExtensions.ReadString),
    };

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

    public ShaderVariantKey GetProgramInvariantKey(ShaderVariantKey variantKey)
    {
        if (variantKey.ShaderHash != SourceHash)
            throw new ArgumentException($"Shader hash in argument does not match this shader instance");
        return new(SourceHash, variantKey.OptionBits & ~ProgramInvarianceMask);
    }

    public string FormatVariantName(ShaderVariantKey variantKey)
    {
        if (variantKey.ShaderHash != SourceHash)
            throw new ArgumentException("Invalid variant key for this shader");
        var name = new StringBuilder();
        int bitOffset = 0;
        foreach (var option in Options)
        {
            var value = (variantKey.OptionBits >> bitOffset) & ((1u << option.BitCount) - 1);
            bitOffset += option.BitCount;

            if (option.NamedValues == null)
            {
                if (value == 0)
                    continue;
                WriteSeparator();
                name.Append(option.Name);
            }
            else
            {
                WriteSeparator();
                name.Append(option.Name);
                name.Append('=');
                if (value < option.NamedValues.Length)
                    name.Append(option.NamedValues[value]);
                else
                {
                    name.Append("unknown");
                    name.Append(value);
                }
            }
        }
        return name.ToString();

        void WriteSeparator()
        {
            if (name.Length != 0)
                name.Append(", ");
        }
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
