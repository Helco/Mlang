using System;
using System.Collections.Generic;

namespace Mlang.Model;

public enum PixelFormat
{
    B8_G8_R8_A8_UNorm,
    B8_G8_R8_A8_UNorm_SRgb,
    R10_G10_B10_A2_UInt,
    R10_G10_B10_A2_UNorm,
    R11_G11_B10_Float,
    R16_Float,
    R16_G16_B16_A16_Float,
    R16_G16_B16_A16_SInt,
    R16_G16_B16_A16_SNorm,
    R16_G16_B16_A16_UInt,
    R16_G16_B16_A16_UNorm,
    R16_G16_Float,
    R16_G16_SInt,
    R16_G16_SNorm,
    R16_G16_UInt,
    R16_G16_UNorm,
    R16_SInt,
    R16_SNorm,
    R16_UInt,
    R16_UNorm,
    R32_Float,
    R32_G32_B32_A32_Float,
    R32_G32_B32_A32_SInt,
    R32_G32_B32_A32_UInt,
    R32_G32_Float,
    R32_G32_SInt,
    R32_G32_UInt,
    R32_SInt,
    R32_UInt,
    R8_G8_B8_A8_SInt,
    R8_G8_B8_A8_SNorm,
    R8_G8_B8_A8_UInt,
    R8_G8_B8_A8_UNorm,
    R8_G8_B8_A8_UNorm_SRgb,
    R8_G8_SInt,
    R8_G8_SNorm,
    R8_G8_UInt,
    R8_G8_UNorm,
    R8_SInt,
    R8_SNorm,
    R8_UInt,
    R8_UNorm,
    D24_UNorm_S8_UInt,
    D32_Float_S8_UInt
}

internal readonly record struct PixelFormatInfo(int NumComponents, ScalarType ScalarType = ScalarType.Float);

internal static class PixelFormatExtensions
{
    private const int DepthOnly = -1;

    public static bool IsDepthOnly(this PixelFormat format) => format.GetInfo().NumComponents == DepthOnly;

    public static int GetComponents(this PixelFormat format) => format.IsDepthOnly() ? 2 : format.GetInfo().NumComponents;

    public static ScalarType GetScalarType(this PixelFormat format) => format.GetInfo().ScalarType;

    public static NumericType GetNumericType(this PixelFormat format) =>
        new(format.GetScalarType(), 1, format.GetComponents());

    private static PixelFormatInfo GetInfo(this PixelFormat format) =>
        Infos.TryGetValue(format, out var info) ? info
        : throw new NotImplementedException($"Unimplemented pixel format: {format}");

    private static readonly IReadOnlyDictionary<PixelFormat, PixelFormatInfo> Infos = new Dictionary<PixelFormat, PixelFormatInfo>
    {
        { PixelFormat.B8_G8_R8_A8_UNorm,        new(4) },
        { PixelFormat.B8_G8_R8_A8_UNorm_SRgb,   new(4) },
        { PixelFormat.R10_G10_B10_A2_UInt,      new(4, ScalarType.UInt) },
        { PixelFormat.R10_G10_B10_A2_UNorm,     new(4) },
        { PixelFormat.R11_G11_B10_Float,        new(3) },
        { PixelFormat.R16_Float,                new(1) },
        { PixelFormat.R16_G16_B16_A16_Float,    new(4) },
        { PixelFormat.R16_G16_B16_A16_SInt,     new(4, ScalarType.Int) },
        { PixelFormat.R16_G16_B16_A16_SNorm,    new(4) },
        { PixelFormat.R16_G16_B16_A16_UInt,     new(4, ScalarType.UInt) },
        { PixelFormat.R16_G16_B16_A16_UNorm,    new(4) },
        { PixelFormat.R16_G16_Float,            new(2) },
        { PixelFormat.R16_G16_SInt,             new(2, ScalarType.Int) },
        { PixelFormat.R16_G16_SNorm,            new(2) },
        { PixelFormat.R16_G16_UInt,             new(2, ScalarType.UInt) },
        { PixelFormat.R16_G16_UNorm,            new(2) },
        { PixelFormat.R16_SInt,                 new(1, ScalarType.Int) },
        { PixelFormat.R16_SNorm,                new(1) },
        { PixelFormat.R16_UInt,                 new(1, ScalarType.UInt) },
        { PixelFormat.R16_UNorm,                new(1) },
        { PixelFormat.R32_Float,                new(1) },
        { PixelFormat.R32_G32_B32_A32_Float,    new(4) },
        { PixelFormat.R32_G32_B32_A32_SInt,     new(4, ScalarType.Int) },
        { PixelFormat.R32_G32_B32_A32_UInt,     new(4, ScalarType.UInt) },
        { PixelFormat.R32_G32_Float,            new(2) },
        { PixelFormat.R32_G32_SInt,             new(2, ScalarType.Int) },
        { PixelFormat.R32_G32_UInt,             new(2, ScalarType.UInt) },
        { PixelFormat.R32_SInt,                 new(1, ScalarType.Int) },
        { PixelFormat.R32_UInt,                 new(1, ScalarType.UInt) },
        { PixelFormat.R8_G8_B8_A8_SInt,         new(4, ScalarType.Int) },
        { PixelFormat.R8_G8_B8_A8_SNorm,        new(4) },
        { PixelFormat.R8_G8_B8_A8_UInt,         new(4, ScalarType.UInt) },
        { PixelFormat.R8_G8_B8_A8_UNorm,        new(4) },
        { PixelFormat.R8_G8_B8_A8_UNorm_SRgb,   new(4) },
        { PixelFormat.R8_G8_SInt,               new(2, ScalarType.Int) },
        { PixelFormat.R8_G8_SNorm,              new(2) },
        { PixelFormat.R8_G8_UInt,               new(2, ScalarType.UInt) },
        { PixelFormat.R8_G8_UNorm,              new(2) },
        { PixelFormat.R8_SInt,                  new(1, ScalarType.Int) },
        { PixelFormat.R8_SNorm,                 new(1) },
        { PixelFormat.R8_UInt,                  new(1, ScalarType.UInt) },
        { PixelFormat.R8_UNorm,                 new(1) },
        { PixelFormat.D24_UNorm_S8_UInt,        new(DepthOnly, ScalarType.UInt) },
        { PixelFormat.D32_Float_S8_UInt,        new(DepthOnly) }
    };
}
