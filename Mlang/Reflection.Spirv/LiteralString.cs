using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mlang.Reflection.Spirv;

/// <summary>
/// A nul-terminated stream of characters consuming an integral number of words. The character set is Unicode in
/// the UTF-8 encoding scheme.The UTF-8 octets (8-bit bytes) are packed four per word, following the little-endian convention
/// (i.e., the first octet is in the lowest-order 8-bits of the word). The final word contains the string’s nul-termination character(0),
/// and all contents past the end of the string in the final word are padded with 0
/// </summary>
public readonly struct LiteralString : IEquatable<LiteralString>
{
    public readonly string Value;

    public int WordCount => Value.Length / 4 + 1; // This is wrong for non-ASCII characters

    public void Write(Span<uint> code, ref int i)
    {
        if (Value == "")
        {
            code[i++] = 0u;
            return;
        }

        var bytes = Encoding.UTF8.GetBytes(Value);
        var j = 0;

        // encoding
        for (; j + 4 <= bytes.Length; j += 4)
            code[i++] = BitConverter.ToUInt32(bytes, j);

        // null termination
        {
            var b0 = j + 0 >= bytes.Length ? (byte)0u : bytes[j + 0];
            var b1 = j + 1 >= bytes.Length ? (byte)0u : bytes[j + 1];
            var b2 = j + 2 >= bytes.Length ? (byte)0u : bytes[j + 2];
            var b3 = j + 3 >= bytes.Length ? (byte)0u : bytes[j + 3];
            var zb = new[] {b0, b1, b2, b3};
            code[i++] = BitConverter.ToUInt32(zb, 0);
        }
    }

    public LiteralString(string value) => Value = value;

    public LiteralString(ReadOnlySpan<uint> code)
    {
        (Value, _) = Parse(code, 0);
    }

    public LiteralString(ReadOnlySpan<uint> code, ref int i)
    {
        (Value, i) = Parse(code, i);
    }

    private static (string, int) Parse(ReadOnlySpan<uint> code, int startI)
    {
        int i = startI;
        for (; i < code.Length && code[i] > 0x01000000; i++) ;

        var uintBuffer = new uint[i - startI + 1];
        for (i = startI; i < startI + uintBuffer.Length; i++)
            uintBuffer[i - startI] = code[i];

        var byteBuffer = MemoryMarshal.AsBytes(uintBuffer.AsSpan());
        var byteLength = byteBuffer.IndexOf((byte)0);
#if NETSTANDARD2_1_OR_GREATER
        var value = Encoding.UTF8.GetString(byteBuffer.Slice(0, byteLength));
#else
        var value = Encoding.UTF8.GetString(byteBuffer.Slice(0, byteLength).ToArray());
#endif
        return (value, i);
    }

    public static implicit operator LiteralString(string str) => new LiteralString(str);

    public override string ToString() => Value == null ? "null" : $"\"{Value}\"";

    public bool Equals(LiteralString other) => Value == other.Value;
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    public override bool Equals(object? obj) => obj is LiteralString @string && Equals(@string);
    public static bool operator ==(LiteralString left, LiteralString right) => left.Equals(right);
    public static bool operator !=(LiteralString left, LiteralString right) => !(left == right);
}
