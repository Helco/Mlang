using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mlang.Model;

public readonly record struct StructureMember(
    string Name,
    int Offset,
    NumericType Type);

public record StructureType : IDataType
{
    /// <remarks>Sorted by offset</remarks>
    public IReadOnlyList<StructureMember> Members { get; }
    public int TotalSize { get; }
    public int Alignment { get; }

    public StructureType(IEnumerable<(string name, NumericType type)> members)
    {
        var newMembers = new List<StructureMember>(members.Count());
        int curOffset = 0;
        foreach (var (name, type) in members)
        {
            if (curOffset % type.Alignment != 0)
                curOffset = Align(curOffset, type.Alignment);
            newMembers.Add(new(name, curOffset, type));
            curOffset += type.Size;
        }

        Members = newMembers;
        Alignment = Members.Max(m => m.Type.Alignment);
        TotalSize = Align(curOffset, Alignment);
    }

    internal static int Align(int offset, int alignment) =>
        (offset + alignment - 1) / alignment * alignment;

    DataTypeCategory IDataType.Category => DataTypeCategory.Structure;

    void IDataType.Write(BinaryWriter writer) => writer.Write(Members, WriteMember);
    private static void WriteMember(BinaryWriter writer, StructureMember member)
    {
        writer.Write(member.Name);
        member.Type.Write(writer);
    }

    internal static StructureType Read(BinaryReader reader) => new(reader.ReadArray(ReadMember));
    private static (string, NumericType) ReadMember(BinaryReader reader) =>
        (reader.ReadString(), NumericType.Read(reader));
}

public readonly record struct BufferType(NumericType Inner) : IDataType
{
    public int Stride => StructureType.Align(Inner.Size, Inner.Alignment);

    DataTypeCategory IDataType.Category => DataTypeCategory.Buffer;

    void IDataType.Write(BinaryWriter writer) => Inner.Write(writer);
    internal static BufferType Read(BinaryReader reader) => new(NumericType.Read(reader));
}
