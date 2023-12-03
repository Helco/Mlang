using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal interface IOptionValueSet
{
    bool TryGetValue(string name, out uint value);
}

internal class DictionaryOptionValueSet : IOptionValueSet
{
    private readonly IReadOnlyDictionary<string, uint> values;
    public DictionaryOptionValueSet(IReadOnlyDictionary<string, uint> values) => this.values = values;
    public bool TryGetValue(string name, out uint value) => values.TryGetValue(name, out value);
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
