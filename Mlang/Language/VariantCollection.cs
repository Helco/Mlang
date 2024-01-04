
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal class VariantCollection : IReadOnlyCollection<IOptionValueSet>
{
    private readonly ASTOption[] options;

    public VariantCollection(ASTOption[] options) => this.options = options;

    public int Count => options.Aggregate(1, (c, o) => c * o.ValueCount);

    public IEnumerator<IOptionValueSet> GetEnumerator()
    {
        var curVariant = 0u;
        while(true)
        {
            yield return new BitsOptionValueSet(options, curVariant);
            var nextVariant = null as uint?;
            foreach (var option in options)
            {
                var bitMask = (1u << option.BitCount) - 1;
                var nextValue = ((curVariant >> option.BitOffset) & bitMask) + 1;
                if (nextValue < option.ValueCount)
                {
                    nextVariant = curVariant & ~((1u << (option.BitCount + option.BitOffset)) - 1);
                    nextVariant |= nextValue << option.BitOffset;
                    break;
                }
            }
            if (nextVariant is null)
                break;
            curVariant = nextVariant.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
