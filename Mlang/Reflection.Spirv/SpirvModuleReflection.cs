using System;
using System.Collections.Generic;
using System.Linq;
using Mlang.Model;

namespace Mlang.Reflection.Spirv;

internal class SpirvModuleReflection
{
    private readonly Dictionary<uint, SpirvInstruction> typesById;
    private readonly Dictionary<uint, SpirvInstruction> variablesById;
    private readonly ILookup<uint, SpirvInstruction> decorations;

    public SpirvModuleReflection(ReadOnlySpan<byte> moduleBytes)
        : this(new SpirvModule(moduleBytes)) { }

    public SpirvModuleReflection(SpirvModule module)
    {
        var relevantInstructions = module.Instructions.TakeWhile(i => i.OpCode != OpCode.OpFunction);
        
        typesById = relevantInstructions
            .Where(i => TypeOpCodes.Contains(i.OpCode))
            .ToDictionary(i => i.Args[0], i => i);
        variablesById = relevantInstructions
            .Where(i => i.OpCode is OpCode.OpVariable)
            .ToDictionary(i => i.Args[1], i => i);
        decorations = relevantInstructions
            .Where(i => DecorateOpCodes.Contains(i.OpCode))
            .ToLookup(i => i.Args[0], i => i);
    }

    public IEnumerable<uint> GetVariablesByClass(StorageClass storageClass) => variablesById
        .Where(p => p.Value.Args[2] == (uint)storageClass)
        .Select(p => p.Key);

    public object? TryConvertTypeOf(uint id, uint? memberIndex)
    {
        uint typeId = variablesById[id].Args[0];
        if (memberIndex != null)
        {
            if (typesById[typeId].OpCode == OpCode.OpTypePointer)
                typeId = typesById[typeId].Args[2];

            if (typesById[typeId].OpCode == OpCode.OpTypeStruct)
                typeId = typesById[typeId].Args[(int)memberIndex.Value + 1];
            else
                return null;
        }
        if (typesById[typeId].OpCode == OpCode.OpTypePointer)
            typeId = typesById[typeId].Args[2];

        return typesById[typeId].OpCode switch
        {
            OpCode.OpTypeStruct => TryConvertStructType(typeId),
            OpCode.OpTypeArray => TryConvertArrayType(typeId),
            OpCode.OpTypeImage => TryConvertImageType(typeId),
            OpCode.OpTypeSampler => TryConvertSamplerType(typeId),
            _ => TryConvertNumericType(typeId)
        };
    }

    private NumericType? TryConvertNumericType(uint typeId)
    {
        int rows = 1, cols = 1;
        if (typesById[typeId].OpCode is OpCode.OpTypeMatrix)
        {
            cols = (int)typesById[typeId].Args[2];
            typeId = typesById[typeId].Args[1];
        }
        if (typesById[typeId].OpCode is OpCode.OpTypeVector)
        {
            rows = (int)typesById[typeId].Args[2];
            typeId = typesById[typeId].Args[1];
        }
        var scalarInstr = typesById[typeId];
        if (scalarInstr.OpCode is not (OpCode.OpTypeInt or OpCode.OpTypeFloat))
            return null;
        var scalarType =
            scalarInstr.OpCode == OpCode.OpTypeFloat ? ScalarType.Float
            : scalarInstr.Args[2] == 0 ? ScalarType.UInt
            : ScalarType.Int;
        var scalarWidth = scalarInstr.Args[1] switch
        {
            8 => ScalarWidth.Byte,
            16 => ScalarWidth.Word,
            32 => ScalarWidth.DWord,
            _ => null as ScalarWidth?
        };
        if (scalarWidth == null)
            return null;
        return new NumericType(scalarType, cols, rows, scalarWidth.Value);
    }

    private SamplerType? TryConvertSamplerType(uint typeId)
    {
        var instr = typesById[typeId];
        if (instr.OpCode != OpCode.OpTypeSampler)
            return null;
        return SamplerType.Normal;
    }

    private ImageType? TryConvertImageType(uint typeId)
    {
        SamplerType? sampler = null;
        if (typesById[typeId].OpCode == OpCode.OpTypeSampledImage)
        {
            sampler = SamplerType.Normal;
            typeId = typesById[typeId].Args[1];
        }
        if (typesById[typeId].OpCode != OpCode.OpTypeImage)
            return null;
        var instr = typesById[typeId];
        var sampled = TryConvertNumericType(instr.Args[1]);

        var dim = (Dim)instr.Args[2];
        var isDepth = instr.Args[3] == 1;
        var isArray = instr.Args[4] == 1;
        var isMS = instr.Args[5] == 1;
        var shape = dim switch
        {
            Dim.Dim1D when !isDepth && !isArray && !isMS => ImageShape._1D,
            Dim.Dim1D when !isDepth && isArray && !isMS => ImageShape._1DArray,
            Dim.Dim2D when !isDepth && !isArray && !isMS => ImageShape._2D,
            Dim.Dim2D when !isDepth && isArray && !isMS => ImageShape._2DArray,
            Dim.Dim2D when !isDepth && !isArray && isMS => ImageShape._2DMS,
            Dim.Dim2D when !isDepth && isArray && isMS => ImageShape._2DMSArray,
            Dim.Dim3D when !isDepth && !isArray && !isMS => ImageShape._3D,
            Dim.Cube when !isDepth && !isArray && !isMS => ImageShape.Cube,
            Dim.Cube when !isDepth && isArray && !isMS => ImageShape.CubeArray,
            _ => null as ImageShape?
        };
        if (sampled == null || !sampled.Value.IsScalar || shape == null)
            return null;

        return new ImageType(sampled.Value.Scalar, shape.Value, sampler);
    }

    public uint? FindDecorationFor(Decoration decoration, uint id) => decorations[id]
        .Where(d => d.OpCode == OpCode.OpDecorate && d.Args[1] == (uint)decoration)
        .Select(d => d.Args[2] as uint?)
        .FirstOrDefault();

    public uint? FindDecorationFor(Decoration decoration, uint id, uint memberIndex) => decorations[id]
        .Where(d => d.OpCode == OpCode.OpMemberDecorate && d.Args[1] == memberIndex && d.Args[2] == (uint)decoration)
        .Select(d => d.Args[3] as uint?)
        .FirstOrDefault();

    public string? FindStringDecorationFor(Decoration decoration, uint id) => decorations[id]
        .Where(d => d.OpCode == OpCode.OpDecorateString && d.Args[1] == (uint)decoration)
        .Select(d => new LiteralString(d.Args[2..]).Value)
        .FirstOrDefault();

    public string? FindStringDecorationFor(Decoration decoration, uint id, uint memberIndex) => decorations[id]
        .Where(d => d.OpCode == OpCode.OpMemberDecorate && d.Args[1] == memberIndex && d.Args[2] == (uint)decoration)
        .Select(d => new LiteralString(d.Args[3..]).Value)
        .FirstOrDefault();

    private static readonly ISet<OpCode> TypeOpCodes = new HashSet<OpCode>()
    {
        OpCode.OpTypeInt,
        OpCode.OpTypeFloat,
        OpCode.OpTypeVector,
        OpCode.OpTypeMatrix,

        OpCode.OpTypeImage, 
        OpCode.OpTypeSampler,
        OpCode.OpTypeSampledImage,

        OpCode.OpTypePointer,
        OpCode.OpTypeStruct,
        OpCode.OpTypeArray,
    };

    private static readonly ISet<OpCode> DecorateOpCodes = new HashSet<OpCode>()
    {
        OpCode.OpDecorate,
        OpCode.OpDecorateString,
        OpCode.OpMemberDecorate,
        OpCode.OpMemberDecorateString
    };
}
