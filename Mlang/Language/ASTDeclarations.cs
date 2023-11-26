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
    public override void Write(CodeWriter writer) => writer.Write(Type.GLSLName);
}

internal class ASTImageType : ASTSimpleType<ImageType>
{
    public override void Write(CodeWriter writer) => writer.Write(Type.GLSLName);
}

internal class ASTSamplerType : ASTSimpleType<SamplerType>
{
    public override void Write(CodeWriter writer) => writer.Write(Type.AsGLSLName());
}

internal class ASTCustomType : ASTSimpleType<string>
{
    public override void Write(CodeWriter writer) => writer.Write(Type);
}

internal class ASTBufferType : ASTType
{
    public required ASTType Inner { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Inner.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("buffer ");
        Inner.Write(writer);
    }
}

internal class ASTArrayType : ASTType
{
    public required ASTType Element { get; init; }
    public required ASTExpression? Size { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Element.Visit(visitor);
        Size?.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        Element.Write(writer);
        writer.Write('[');
        Size?.Write(writer);
        writer.Write(']');
    }
}

internal class ASTDeclaration : ASTNode
{
    public required ASTType Type { get; init; }
    public required string Name { get; init; }
    public ASTExpression? Initializer { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Type.Visit(visitor);
        Initializer?.Visit(visitor);
    }

    public void WriteWithoutType(CodeWriter writer)
    {
        writer.Write(Name);
        if (Initializer != null)
        {
            writer.Write(" = ");
            Initializer.Write(writer);
        }
    }

    public override void Write(CodeWriter writer)
    {
        Type.Write(writer);
        writer.Write(' ');
        WriteWithoutType(writer);
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
        visitor.Visit(this);
        ReturnType?.Visit(visitor);
        foreach (var param in Parameters)
            param.Visit(visitor);
        Body?.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        if (ReturnType == null)
            writer.Write("void");
        else
            ReturnType.Write(writer);
        writer.Write(' ');
        writer.Write(Name);
        writer.Write('(');

        if (Parameters.Any())
            Parameters.First().Write(writer);
        foreach (var param in Parameters.Skip(1))
        {
            writer.Write(", ");
            param.Write(writer);
        }
        writer.Write(')');

        if (Body == null)
            writer.WriteLine(";");
        else
        {
            writer.WriteLine();
            Body.WriteAsNewScope(writer);
        }
    }
}

internal class ASTStorageBlock : ASTGlobalBlock
{
    public required TokenKind StorageKind { get; init; }
    public required ASTExpression? Condition { get; init; }
    public required ASTDeclaration[] Declarations { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Condition?.Visit(visitor);
        foreach (var decl in Declarations)
            decl.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write(StorageKind switch
        {
            TokenKind.KwAttributes => "attributes",
            TokenKind.KwInstances => "instances",
            TokenKind.KwUniform => "uniform",
            TokenKind.KwVarying => "varying",
            _ => throw new InvalidOperationException("Invalid storage kind in StorageBlock")
        });
        if (Condition != null)
        {
            writer.Write(" if (");
            Condition.Write(writer);
            writer.Write(')');
        }
        writer.WriteLine();
        writer.WriteLine("{");
        using var indented = writer.Indented;
        foreach (var decl in Declarations)
        {
            decl.Write(indented);
            indented.WriteLine();
        }
        writer.WriteLine("}");
    }
}

internal class ASTStageBlock : ASTGlobalBlock
{
    public required TokenKind Stage { get; init; }
    public required ASTFunction[] Functions { get; init; }
    public required ASTStatement[] Statements { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        foreach (var func in Functions)
            func.Visit(visitor);
        foreach (var stmt in Statements)
            stmt.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.WriteLine(Stage switch
        {
            TokenKind.KwVertex => "vertex",
            TokenKind.KwFragment => "fragment",
            _ => throw new InvalidOperationException("Invalid stage kind in StageBlock")
        });
        writer.WriteLine("{");

        using var indented = writer.Indented;
        foreach (var func in Functions)
        {
            func.Write(indented);
            indented.WriteLine();
        }
        foreach (var stmt in Statements)
        {
            stmt.Write(indented);
        }

        writer.WriteLine("}");
    }
}


