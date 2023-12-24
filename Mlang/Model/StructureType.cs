using System;
using System.Collections.Generic;

namespace Mlang.Model;

public readonly record struct StructureMember(
    string Name,
    int Offset,
    int Size,
    NumericType Type);

public record StructureType
{
    public required IReadOnlyList<StructureMember> Members { get; init; }
    public required int TotalSize { get; init; }
}
