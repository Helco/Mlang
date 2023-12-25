using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using cilspirv.Spirv;

namespace Mlang.Reflection.Spirv;

public readonly record struct SpirvInstruction
{
    private readonly byte[] buffer;
    private readonly Range range;

    public int WordIndex => range.Start.Value;
    public ReadOnlySpan<uint> Words => MemoryMarshal.Cast<byte, uint>(buffer)[range];
    public OpCode OpCode => (OpCode)(Words[0] & 0xFFFF);
    public ReadOnlySpan<uint> Args => Words[1..];

    public SpirvInstruction(byte[] buffer, Range range)
    {
        this.buffer = buffer;
        this.range = range;
    }
}

internal class SpirvInstructionList : IReadOnlyCollection<SpirvInstruction>
{
    private readonly byte[] wordBytes;
    private readonly Lazy<int> count;
    private Span<uint> Words => MemoryMarshal.Cast<byte, uint>(wordBytes);
    public int Count => count.Value;

    public SpirvInstructionList(Stream stream, bool needsSwapping)
    {
        var wordBytesSize = stream.Length - stream.Position;
        wordBytes = new byte[wordBytesSize];
        if (stream.Read(wordBytes, 0, wordBytes.Length) != wordBytesSize)
            throw new EndOfStreamException($"Could not read all {wordBytesSize} bytes of the SPIRV instructions");

        if (needsSwapping)
            SpirvModule.SwapAll(Words);

        count = new(() => System.Linq.Enumerable.Count(this));
    }

    public IEnumerator<SpirvInstruction> GetEnumerator()
    {
        int index = 0;
        while (index < Words.Length)
        {
            var wordCount = (int)(Words[index] >> 16);
            yield return new(wordBytes, index..(index + wordCount));
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
