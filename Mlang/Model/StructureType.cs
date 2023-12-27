using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Model;

public readonly record struct StructureMember(
    string Name,
    int Offset,
    NumericType Type);

public record StructureType
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
}

public readonly record struct BufferType(NumericType Inner)
{
    public int Stride => StructureType.Align(Inner.Size, Inner.Alignment);
}
