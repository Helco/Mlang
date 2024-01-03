using System;
using System.Collections.Generic;
using System.IO;
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

public readonly record struct SamplerType : IDataType
{
    DataTypeCategory IDataType.Category => DataTypeCategory.Sampler;

    void IDataType.Write(BinaryWriter writer)
    {
    }

    public string GLSLName => "sampler";

    public static readonly SamplerType Normal = default;

    public static bool TryParse(string text, out SamplerType type)
    {
        type = default;
        if (text == SamplerType.Normal.GLSLName) type = SamplerType.Normal;
        else return false;
        return true;
    }
}

public readonly record struct ImageType(
    ScalarType Scalar,
    ImageShape Shape,
    SamplerType? Sampler)
    : IDataType
{
    public bool IsValid => true;

    public string GLSLName
    {
        get
        {
            if (!IsValid)
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
            return name.ToString();
        }
    }

    public override string ToString() => GLSLName;

    public static bool TryParse(string text, out ImageType type) =>
        TypeNames.TryGetValue(text, out type);

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
            }
        }
        return types;

        void Add(ImageType type)
        {
            if (type.IsValid)
                types.Add(type.GLSLName, type);
        }
    }

    DataTypeCategory IDataType.Category => DataTypeCategory.Image;

    void IDataType.Write(BinaryWriter writer)
    {
        writer.Write((byte)Scalar);
        writer.Write((byte)Shape);
        writer.Write(Sampler.HasValue);
    }

    internal static ImageType Read(BinaryReader reader) => new(
        (ScalarType)reader.ReadByte(),
        (ImageShape)reader.ReadByte(),
        reader.ReadBoolean() ? SamplerType.Normal : null);
}
