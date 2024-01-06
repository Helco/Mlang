using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mlang.Compiler")]
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

    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value) =>
        (key, value) = (pair.Key, pair.Value);

    public static IEnumerable<(int, T)> Indexed<T>(this IEnumerable<T> set) =>
        set.Select((val, index) => (index, val));

    public static Vector4 ReadVector4(this BinaryReader reader) => new(
        reader.ReadSingle(),
        reader.ReadSingle(),
        reader.ReadSingle(),
        reader.ReadSingle());

    public static void Write(this BinaryWriter writer, Vector4 v)
    {
        writer.Write(v.X);
        writer.Write(v.Y);
        writer.Write(v.Z);
        writer.Write(v.W);
    }

    public static T[] ReadArray<T>(this BinaryReader reader, Func<BinaryReader, T> readFunc)
    {
        var count = reader.ReadUInt32();
        var array = new T[checked((int)count)];
        for (uint i = 0; i < count; i++)
            array[i] = readFunc(reader);
        return array;
    }

    public static void Write<T>(this BinaryWriter writer, IReadOnlyList<T> array, Action<BinaryWriter, T> writeFunc)
    {
        writer.Write(array.Count);
        foreach (var e in array)
            writeFunc(writer, e);
    }

    public static T? ReadNullable<T>(this BinaryReader reader, Func<BinaryReader, T> readFunc) where T : struct =>
        reader.ReadBoolean() ? readFunc(reader) : default(T?);

    public static void Write<T>(this BinaryWriter writer, T? opt, Action<BinaryWriter, T> writeFunc) where T : struct
    {
        writer.Write(opt.HasValue);
        if (opt != null)
            writeFunc(writer, opt.Value);
    }

    // to be used for reading/writing arrays
    public static void Write(BinaryWriter writer, string value) => writer.Write(value);
    public static string ReadString(BinaryReader reader) => reader.ReadString();
}
