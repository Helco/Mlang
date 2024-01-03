using System;
using System.IO;

namespace Mlang.Model;

internal enum DataTypeCategory
{
    Numeric,
    Structure,
    Image,
    Sampler,
    Buffer
}

internal interface IDataType
{
    DataTypeCategory Category { get; }

    void Write(BinaryWriter writer);
}

public interface IBindingInfo
{
    int SetIndex { get; }
    int BindingIndex { get; }
    string Name { get; }
    bool IsInstance { get; }
    object Type { get; }
}

public record BindingInfo<T>: IBindingInfo where T : notnull
{
    public int SetIndex { get; }
    public int BindingIndex { get; }
    public string Name { get; }
    public T Type { get; }
    object IBindingInfo.Type => Type;
    public bool IsInstance { get; }

    internal BindingInfo(int set, int binding, string name, T type, bool isInstance = false)
    {
        SetIndex = set;
        BindingIndex = binding;
        Name = name;
        Type = type;
        IsInstance = isInstance;
        if (type is not IDataType)
            throw new ArgumentException($"Invalid type for BindingInfo: {typeof(T)}");
    }
}

internal static class BindingInfo
{
    public static void Write(BinaryWriter writer, IBindingInfo info)
    {
        writer.Write(info.SetIndex);
        writer.Write(info.BindingIndex);
        writer.Write(info.Name);
        writer.Write(info.IsInstance);

        var type = (IDataType)info.Type;
        writer.Write((byte)type.Category);
        type.Write(writer);
    }

    public static IBindingInfo Read(BinaryReader reader)
    {
        var set= reader.ReadInt32();
        var binding= reader.ReadInt32();
        var name = reader.ReadString();
        var isInstance = reader.ReadBoolean();
        var category = (DataTypeCategory)reader.ReadByte();
        return category switch
        {
            DataTypeCategory.Structure => new BindingInfo<StructureType>(set, binding, name, StructureType.Read(reader), isInstance),
            DataTypeCategory.Image => new BindingInfo<ImageType>(set, binding, name, ImageType.Read(reader), isInstance),
            DataTypeCategory.Sampler => new BindingInfo<SamplerType>(set, binding, name, default, isInstance),
            DataTypeCategory.Buffer => new BindingInfo<BufferType>(set, binding, name, BufferType.Read(reader), isInstance),
            _ => throw new InvalidDataException($"Invalid data type category for binding")
        };
    }
}
