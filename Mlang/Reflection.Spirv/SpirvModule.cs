using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace Mlang.Reflection.Spirv
{
    public sealed record SpirvModule
    {
        public const uint BoundNotSet = uint.MaxValue;
        public const uint DefaultMagic = 0x07230203;
        public const uint SwappedMagic = 0x03022307;

        public uint Magic { get; } = DefaultMagic;
        public Version SpirvVersion { get; } = new Version(1, 4);
        public ushort GeneratorToolID { get; }
        public Version GeneratorVersion { get; }
        public uint Bound { get; } = BoundNotSet;
        public IReadOnlyCollection<SpirvInstruction> Instructions { get; }

        public SpirvModule(ReadOnlySpan<byte> spirvBytes)
        {
            Magic = ReadWord(ref spirvBytes, needsSwap: false);
            var needsSwapping = Magic switch
            {
                DefaultMagic => false,
                SwappedMagic => true,
                _ => throw new InvalidDataException($"Invalid magic number {Magic.ToString("X8")}")
            };

            SpirvVersion = DecodeVersion(ReadWord(ref spirvBytes, needsSwapping) >> 8);
            uint generatorMagic = ReadWord(ref spirvBytes, needsSwapping);
            GeneratorToolID = (ushort)(generatorMagic >> 16);
            GeneratorVersion = DecodeVersion(generatorMagic);
            Bound = ReadWord(ref spirvBytes, needsSwapping);
            if (ReadWord(ref spirvBytes, needsSwapping) != 0)
                throw new NotSupportedException("Unsupported reserved number");

            Instructions = new SpirvInstructionList(spirvBytes, needsSwapping: Magic == SwappedMagic);
        }

        private uint ReadWord(ref ReadOnlySpan<byte> bytes, bool needsSwap)
        {
            var value = MemoryMarshal.Cast<byte, uint>(bytes)[0];
            bytes = bytes[sizeof(uint)..];
            return needsSwap ? Swap32(value) : value;
        }

        // from https://stackoverflow.com/questions/19560436/bitwise-endian-swap-for-various-types
        private static uint Swap32(uint x)
        {
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            // swap adjacent 8-bit blocks
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        internal static void SwapAll(Span<uint> words)
        {
            // Let's leave the unnecessary but certainly fun performance optimization for later
            foreach (ref var word in words)
                word = Swap32(word);
        }

        private static Version DecodeVersion(uint v) => new Version((int)(v >> 8) & 0xff, (int)(v & 0xff));
    }
}
