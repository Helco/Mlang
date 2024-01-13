using System;
using System.Collections.Generic;
using System.Linq;
using Mlang.Model;

namespace Mlang.Language;

internal abstract class ASTSimpleType<T> : ASTType
{
    public required T Type { get; init; }
}

internal class ASTNumericType : ASTSimpleType<NumericType>
{
    public override bool IsBindingType => false;
}

internal class ASTImageType : ASTSimpleType<ImageType>
{
    public override bool IsBindingType => true;
}

internal class ASTSamplerType : ASTSimpleType<SamplerType>
{
    public override bool IsBindingType => true;
}

internal class ASTCustomType : ASTSimpleType<string>
{
    public override bool IsBindingType => false;
}

internal class ASTBufferType : ASTType
{
    public override bool IsBindingType => true;
    public required ASTType Inner { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Inner.Visit(visitor);
    }
}

internal class ASTArrayType : ASTType
{
    public override bool IsBindingType => false;
    public required ASTType Element { get; init; }
    public required ASTExpression? Size { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Element.Visit(visitor);
        Size?.Visit(visitor);
    }
}

internal readonly struct ASTLayoutInfo
{
    public readonly int? InLocation;
    public readonly int? OutLocation;
    public readonly int? Binding;
    public readonly int? Set;

    private ASTLayoutInfo(int? inLocation = null, int? outLocation = null, int? binding = null, int? set = null)
    {
        InLocation = inLocation;
        OutLocation = outLocation;
        Binding = binding;
        Set = set;
    }

    public static ASTLayoutInfo CreateInLocation(int location) => new(inLocation: location);
    public static ASTLayoutInfo CreateLocation(int? location) => new(location, location);
    public ASTLayoutInfo WithOutLocation(int outLocation) => new(InLocation, outLocation, Binding, Set);
    public static ASTLayoutInfo CreateBinding(int set, int binding) => new(binding: binding, set: set);
}

internal class ASTDeclaration : ASTNode
{
    public required ASTType Type { get; init; }
    public required string Name { get; init; }
    public ASTExpression? Initializer { get; init; }
    public ASTLayoutInfo Layout { get; set; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Type.Visit(visitor);
        Initializer?.Visit(visitor);
    }
}

internal class ASTFunction : ASTGlobalBlock
{
    public required ASTType? ReturnType { get; init; }
    public required string Name { get; init; }
    public required ASTDeclaration[] Parameters { get; init; }
    public ASTStatement? Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        ReturnType?.Visit(visitor);
        foreach (var param in Parameters)
            param.Visit(visitor);
        Body?.Visit(visitor);
    }
}

internal class ASTStorageBlock : ASTConditionalGlobalBlock
{
    public string? UserName { get; init; }
    public required TokenKind StorageKind { get; init; }
    public required ASTDeclaration[] Declarations { get; init; }

    public string NameForGLSL => UserName ?? $"block_{Range.Start.Line + 1}_{Range.Start.Column + 1}";
    public string NameForReflection => UserName ?? (Declarations.Length == 1
        ? Declarations.Single().Name
        : $"block_{Range.Start.Line + 1}_{Range.Start.Column + 1}");

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Condition?.Visit(visitor);
        foreach (var decl in Declarations)
            decl.Visit(visitor);
    }
}

internal class ASTStageBlock : ASTConditionalGlobalBlock
{
    public required TokenKind Stage { get; init; }
    public required ASTFunction[] Functions { get; init; }
    public required ASTStatement[] Statements { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Condition?.Visit(visitor);
        foreach (var func in Functions)
            func.Visit(visitor);
        foreach (var stmt in Statements)
            stmt.Visit(visitor);
    }
}

internal class ASTPipelineBlock : ASTConditionalGlobalBlock
{
    public required PartialPipelineState State { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Condition?.Visit(visitor);
    }
}

internal class ASTOption : ASTGlobalBlock
{
    public required int Index { get; init; }
    public required int BitOffset { get; init; }
    public required string Name { get; init; }
    public required string[]? NamedValues { get; init; }
    public bool IsProgramInvariant { get; set; }
    public int ValueCount => NamedValues?.Length ?? 2;
    public int BitCount => GetBitCount(ValueCount);

    public static int GetBitCount(int valueCount) =>
        (int)Math.Ceiling(Math.Log(valueCount) / Math.Log(2));
}
