using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal interface IOptionValueSet
{
    bool TryGetValue(string name, out uint value);
}

internal static class IOptionValueSetExtensions
{
    public static uint GetValue(this IOptionValueSet set, string name) =>
        set.TryGetValue(name, out var value) ? value
        : throw new ArgumentException($"Could not retrieve option value {name}");

    public static bool GetBool(this IOptionValueSet set, string name, bool required = false) => required
        ? set.GetValue(name) != 0
        : set.TryGetValue(name, out var value) ? value != 0 : false;
}

internal class DictionaryOptionValueSet : IOptionValueSet
{
    private readonly IReadOnlyDictionary<string, uint> values;
    public DictionaryOptionValueSet(IReadOnlyDictionary<string, uint> values) => this.values = values;
    public bool TryGetValue(string name, out uint value) => values.TryGetValue(name, out value);
}

internal class BitsOptionValueSet : IOptionValueSet
{
    private readonly ASTOption[] options;
    private readonly uint bits;

    public uint OptionBits => bits;

    public BitsOptionValueSet(ASTOption[] options, uint bits)
    {
        this.options = options;
        this.bits = bits;
    }

    public bool TryGetValue(string name, out uint value)
    {
        value = 0;
        var option = options.FirstOrDefault(o => o.Name == name);
        if (option == null || option.BitOffset >= sizeof(uint) * 8)
            return false;

        value = (bits >> option.BitOffset) & ((1u << option.BitCount) - 1);
        if (option.NamedValues?.Length > 0)
            value = (uint)Math.Min(value, option.NamedValues.Length - 1);
        return true;
    }
}

internal class FilteredOptionValueSet : IOptionValueSet
{
    private readonly ASTOption[] options;
    private readonly Dictionary<string, uint> valueNames;
    private readonly IOptionValueSet? parent;

    internal bool AccessedOption { get; set; }

    public FilteredOptionValueSet(ASTOption[] options, IOptionValueSet? parent = null)
    {
        this.options = options;
        this.parent = parent;
        valueNames = options
            .Where(o => o.NamedValues != null)
            .SelectMany(o => o.NamedValues.Select((name, index) => (name, index)))
            .ToDictionary(t => t.name, t => (uint)t.index);
    }

    public bool TryGetValue(string name, out uint value)
    {
        if (valueNames.TryGetValue(name, out value))
            return true;

        var option = options.FirstOrDefault(o => o.Name == name);
        if (option != null)
        {
            AccessedOption = true;
            parent?.TryGetValue(name, out value);
            return true;
        }

        return false;
    }
}
