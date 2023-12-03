using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal class FilteredOptionValueSet : IOptionValueSet
{
    private readonly ASTOption[] options;
    private readonly Dictionary<string, uint> valueNames;
    private readonly IReadOnlyDictionary<string, uint>? values;

    internal bool AccessedOption { get; set; }

    public FilteredOptionValueSet(ASTOption[] options, IReadOnlyDictionary<string, uint>? values = null)
    {
        this.options = options;
        this.values = values;
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
            values?.TryGetValue(name, out value);
            return true;
        }

        return false;
    }
}
