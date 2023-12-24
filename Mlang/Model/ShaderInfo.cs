using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Mlang.Model;

public record OptionInfo(string name, string[]? namedValues = null)
{
    public string Name => name;
    public string[]? NamedValues => namedValues;
}

public record VertexAttributeInfo(int attrIndex, string name, NumericType type, bool isInstance = false)
{
    public int AttributeIndex => attrIndex;
    public string Name => name;
    public NumericType Type => type;
    public bool IsInstance => isInstance;
}

public interface IBindingInfo
{
    int SetIndex { get; }
    int BindingIndex { get; }
    string Name { get; }
    object Type { get; }
    bool IsInstance { get; }
}

public record BindingInfo<T>(int set, int binding, string name, T type, bool isInstance = false) : IBindingInfo where T : struct
{
    public int SetIndex => set;
    public int BindingIndex => binding;
    public string Name => name;
    public T Type => type;
    object IBindingInfo.Type => type;
    public bool IsInstance => isInstance;
}

public record ShaderInfo
{
    public required IReadOnlyList<OptionInfo> Options { get; init; }
    public required IReadOnlyList<string> VertexAttributes { get; init; }
    public required IReadOnlyList<string> InstanceAttributes { get; init; }
    public required IReadOnlyList<string> Bindings { get; init; }
}

public record ShaderVariantInfo
{
    /// <remarks>Sorted by attribute index</remarks>
    public required IReadOnlyList<VertexAttributeInfo> VertexAttributes { get; init; }
    public required IReadOnlyList<int[]> BindingSetSizes { get; init; }
    /// <remarks>Sorted by set and binding indices</remarks>
    public required IReadOnlyList<IBindingInfo> Bindings { get; init; }
}
