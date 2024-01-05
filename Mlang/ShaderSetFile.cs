using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mlang.Model;

namespace Mlang;

internal static class ShaderSetFile
{
    internal const uint Magic = 0x4D4C5353; // "MLang Shader Set"
    internal const uint Version = 0;

    internal struct ShaderHeader
    {
        public ShaderInfo Info;
        public string Name;
        public string? Source;
        public uint VariantOffset;
        public uint VariantCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = sizeof(uint))]
    internal struct VariantHeader
    {
        public uint OptionBits;
        public uint Offset;
    }
}