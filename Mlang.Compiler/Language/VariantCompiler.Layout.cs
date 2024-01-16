using System;
using System.Collections.Generic;
#if NETSTANDARD2_0
using System.Collections.Generic.Polyfill;
#endif
using System.Linq;
using Mlang.Model;
using static Mlang.Diagnostics;
using static Mlang.Language.ShaderCompiler;

namespace Mlang.Language;

internal readonly struct LayoutInfo
{
    public readonly int? InLocation;
    public readonly int? OutLocation;
    public readonly int? Binding;
    public readonly int? Set;

    private LayoutInfo(int? inLocation = null, int? outLocation = null, int? binding = null, int? set = null)
    {
        InLocation = inLocation;
        OutLocation = outLocation;
        Binding = binding;
        Set = set;
    }

    public static LayoutInfo CreateInLocation(int location) => new(inLocation: location);
    public static LayoutInfo CreateLocation(int? location) => new(location, location);
    public LayoutInfo WithOutLocation(int outLocation) => new(InLocation, outLocation, Binding, Set);
    public static LayoutInfo CreateBinding(int set, int binding) => new(binding: binding, set: set);
}

partial class VariantCompiler
{
    private IEnumerable<ASTStorageBlock> GetStorageBlocksOfKind(TokenKind storageKind) =>
        unit.Blocks.OfType<ASTStorageBlock>().Where(b => b.StorageKind == storageKind);

    private IReadOnlyList<VertexAttributeInfo> LayoutVertexAttributes(IOptionValueSet optionValues)
    {
        var blocks = GetStorageBlocksOfKind(TokenKind.KwAttributes);
        if (optionValues.GetBool(IsInstancedOptionName))
            blocks = blocks.Concat(GetStorageBlocksOfKind(TokenKind.KwInstances));
        blocks = blocks.Where(b => b.EvaluateCondition(optionValues));

        int location = 0;
        var infos = new List<VertexAttributeInfo>(blocks.Sum(b => b.Declarations.Length));
        foreach (var block in blocks)
        {
            foreach (var decl in block.Declarations)
            {
                NumericType numericType = default;
                if (decl.Type is ASTNumericType astNumericType)
                    numericType = astNumericType.Type;
                else
                    diagnostics.Add(DiagNonNumericVertexAttribute(sourceFile, decl));

                layoutInfos[decl] = LayoutInfo.CreateInLocation(location);
                infos.Add(new()
                {
                    AttributeIndex = location,
                    Name = decl.Name,
                    Type = numericType,
                    IsInstance = block.StorageKind == TokenKind.KwInstances
                });
                location += numericType.Columns;
            }
        }
        return infos;
    }

    private (IReadOnlyList<int>, IReadOnlyList<IBindingInfo>) LayoutBindings(IOptionValueSet optionValues)
    {
        var blocks = GetStorageBlocksOfKind(TokenKind.KwUniform);
        if (!optionValues.GetBool(IsInstancedOptionName))
            blocks = blocks.Concat(GetStorageBlocksOfKind(TokenKind.KwInstances));
        blocks = blocks.Where(b => b.EvaluateCondition(optionValues));

        int set = 0, binding = 0; // not sure when or how set should be increased
        var setSizes = new List<int>();
        var infos = new List<IBindingInfo>();
        foreach (var block in blocks)
        {
            var isInstance = block.StorageKind == TokenKind.KwInstances;
            int? structBinding = null;
            var structMembers = new List<(string name, NumericType type)>();
            foreach (var decl in block.Declarations)
            {
                if (decl.Type.IsBindingType)
                {
                    layoutInfos[decl] = LayoutInfo.CreateBinding(set, binding);
                    infos.Add(decl.Type switch
                    {
                        ASTImageType type => new BindingInfo<ImageType>(set, binding, decl.Name, type.Type),
                        ASTSamplerType type => new BindingInfo<SamplerType>(set, binding, decl.Name, type.Type),
                        ASTBufferType type => LayoutBufferBinding(set, binding, decl),
                        _ => throw new NotImplementedException($"Unimplemented type category for bindings layout {decl.Type}")
                    });
                    binding++;
                }
                else if (decl.Type is ASTNumericType numericType)
                {
                    structBinding ??= binding++;
                    structMembers.Add((decl.Name, numericType.Type));
                    layoutInfos[decl] = LayoutInfo.CreateBinding(set, structBinding.Value);
                }
                else
                    diagnostics.Add(DiagNonNumericNorBindingUniform(sourceFile, decl));
            }
            if (structBinding != null)
            {
                var structType = new StructureType(structMembers);
                infos.Add(new BindingInfo<StructureType>(set, structBinding.Value, block.NameForReflection, structType, isInstance));
            }
        }
        setSizes.Add(binding);
        return (setSizes, infos);
    }

    private IBindingInfo LayoutBufferBinding(int set, int binding, ASTDeclaration decl)
    {
        if (decl.Type is not ASTBufferType { Inner: ASTArrayType { Element: ASTNumericType numericType } })
        {
            diagnostics.Add(DiagUnsupportedBufferType(sourceFile, decl));
            return new BindingInfo<int>(set, binding, decl.Name, 0);
        }

        var type = new BufferType(numericType.Type);
        return new BindingInfo<BufferType>(set, binding, decl.Name, type);
    }

    private void LayoutVarying(IOptionValueSet optionValues, ISet<ASTDeclaration> transferredInstanceVars)
    {
        var blocks = GetStorageBlocksOfKind(TokenKind.KwVarying).Where(b => b.EvaluateCondition(optionValues));
        int location = 0;
        foreach (var decl in blocks.SelectMany(b => b.Declarations))
            layoutInfos[decl] = LayoutInfo.CreateLocation(location++);
        foreach (var decl in transferredInstanceVars)
            layoutInfos[decl] = layoutInfos.GetValueOrDefault(decl).WithOutLocation(location++);
    }
}
