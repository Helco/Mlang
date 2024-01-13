
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal class VariantCollection : IReadOnlyCollection<IOptionValueSet>
{
    private readonly bool? onlyWithVariance;
    private readonly uint baseOptionBits;
    private readonly ASTOption[] allOptions;

    private IEnumerable<ASTOption> RelevantOptions => onlyWithVariance.HasValue
        ? allOptions.Where(o => o.IsProgramInvariant == onlyWithVariance)
        : allOptions;

    public VariantCollection(IEnumerable<ASTOption> options, bool? onlyWithVariance = null, uint baseOptionBits = 0)
    {
        allOptions = options.OrderBy(o => o.BitOffset).ToArray();
        this.onlyWithVariance = onlyWithVariance;
        this.baseOptionBits = baseOptionBits;
    }

    public int Count => RelevantOptions.Aggregate(1, (c, o) => c * o.ValueCount);

    public IEnumerator<IOptionValueSet> GetEnumerator()
    {
        var curVariant = 0u;
        while(true)
        {
            yield return new BitsOptionValueSet(allOptions, curVariant | baseOptionBits);
            var nextVariant = null as uint?;
            foreach (var option in RelevantOptions)
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
