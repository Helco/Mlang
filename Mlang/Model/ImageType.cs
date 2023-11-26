using System;
using System.Collections.Generic;
using System.Text;

namespace Mlang.Model;

public enum ImageShape
{
    _1D,
    _2D,
    _3D,
    Cube,
    _1DArray,
    _2DArray,
    CubeArray,
    _2DMS,
    _2DMSArray,
}

public enum SamplerType
{
    Normal,
    Shadow
}

public static class SamplerTypeExtensions
{
    public static string AsGLSLName(this SamplerType type) => type switch
    {
        SamplerType.Normal => "sampler",
        SamplerType.Shadow => "samplerShadow",
        _ => throw new NotImplementedException("Unimplemented sampler type")
    };
}

public readonly record struct ImageType(
    ScalarType Scalar,
    ImageShape Shape,
    SamplerType? Sampler)
{
    public bool IsInvalid =>
        Sampler == SamplerType.Shadow &&
        Shape is ImageShape._3D or ImageShape._2DMS or ImageShape._2DMSArray;

    public string GLSLName
    {
        get
        {
            if (IsInvalid)
                return "<invalid-image-type>";
            var name = new StringBuilder(24);
            name.Append(Scalar.AsPrefix());
            name.Append(Sampler == null ? "texture" : "sampler");
            name.Append(Shape switch
            {
                ImageShape._1D => "1D",
                ImageShape._2D => "2D",
                ImageShape._3D => "3D",
                ImageShape.Cube => "Cube",
                ImageShape._1DArray => "1DArray",
                ImageShape._2DArray => "2DArray",
                ImageShape.CubeArray => "CubeArray",
                ImageShape._2DMS => "2DMS",
                ImageShape._2DMSArray => "2DMSArray",
                _ => throw new NotImplementedException("Unimplemented image shape")
            });
            if (Sampler == SamplerType.Shadow)
                name.Append("Shadow");
            return name.ToString();
        }
    }

    public bool TryParse(string text, out ImageType type) =>
        TypeNames.TryGetValue(text, out type);

    public bool TryParseSamplerType(string text, out SamplerType type)
    {
        type = default;
        if (text == SamplerType.Normal.AsGLSLName()) type = SamplerType.Normal;
        else if (text == SamplerType.Shadow.AsGLSLName()) type = SamplerType.Shadow;
        else return false;
        return true;
    }

    private static readonly IReadOnlyDictionary<string, ImageType> TypeNames = GenerateAll();

    private static IReadOnlyDictionary<string, ImageType> GenerateAll()
    {
        var types = new Dictionary<string, ImageType>();
        foreach(ScalarType scalarType in Enum.GetValues(typeof(ScalarType)))
        {
            foreach (ImageShape imageShape in Enum.GetValues(typeof(ImageShape)))
            {
                Add(new(scalarType, imageShape, null));
                Add(new(scalarType, imageShape, SamplerType.Normal));
                Add(new(scalarType, imageShape, SamplerType.Shadow));
            }
        }
        return types;

        void Add(ImageType type)
        {
            if (type.IsInvalid)
                return;
            types.Add(type.GLSLName, type);
        }
    }
}
