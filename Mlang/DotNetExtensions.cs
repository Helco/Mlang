using System;
using System.Collections.Generic;
using System.Linq;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mlangc")]

namespace Mlang;

internal static class DotNetExtensions
{
    public static ulong Sum(this IEnumerable<ulong> set) =>
        set.Aggregate((a, b) => a + b);

    public static ulong Sum<T>(this IEnumerable<T> set, Func<T, ulong> getter) =>
        set.Select(getter).Sum();

    public static bool None<T>(this IEnumerable<T> set) => !set.Any();

    public static bool None<T>(this IEnumerable<T> set, Func<T, bool> filter) => !set.Any(filter);

    public static IEnumerable<(T1, T2)> Zip<T1, T2>(this IEnumerable<T1> set1, IEnumerable<T2> set2) =>
        set1.Zip(set2, (a, b) => (a, b));
}
