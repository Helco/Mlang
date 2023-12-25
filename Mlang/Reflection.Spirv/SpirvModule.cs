using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using Mlang.Reflection.Spirv;

namespace cilspirv.Spirv
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

        public SpirvModule(Stream stream, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen);
            Magic = reader.ReadUInt32();

            var readWord = Magic switch
            {
                DefaultMagic => (Func<uint>)reader.ReadUInt32,
                SwappedMagic => () => Swap32(reader.ReadUInt32()),
                _ => throw new InvalidDataException($"Invalid magic number {Magic.ToString("X8")}")
            };
            SpirvVersion = DecodeVersion(readWord() >> 8);
            uint generatorMagic = readWord();
            GeneratorToolID = (ushort)(generatorMagic >> 16);
            GeneratorVersion = DecodeVersion(generatorMagic);
            Bound = readWord();
            if (readWord() != 0)
                throw new NotSupportedException("Unsupported reserved number");

            Instructions = new SpirvInstructionList(stream, needsSwapping: Magic == SwappedMagic);
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
