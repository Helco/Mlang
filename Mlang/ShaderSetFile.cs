﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
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
        public int VariantOffset;
        public int VariantCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = sizeof(uint))]
    internal struct VariantHeader
    {
        public uint OptionBits;
        public uint Offset;
    }
}