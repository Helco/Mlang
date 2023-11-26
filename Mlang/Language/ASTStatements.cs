using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal class ASTEmptyStatement : ASTStatement
{
    public override void Write(CodeWriter writer) => writer.WriteLine(";");
}

internal class ASTDeclarationStatement : ASTStatement
{
    public ASTType Type => Declarations.First().Type;
    public required ASTDeclaration[] Declarations { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Type.Visit(visitor);
        foreach (var declaration in Declarations.Where(t => t.Initializer != null))
            declaration.Initializer!.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        Type.Write(writer);
        writer.Write(' ');
        if (Declarations.Any())
            Declarations.First().WriteWithoutType(writer);
        foreach (var declaration in Declarations.Skip(1))
        {
            writer.Write(", ");
            declaration.WriteWithoutType(writer);
        }
        writer.WriteLine(";");
    }
}

internal class ASTExpressionStatement : ASTStatement
{
    public required ASTExpression Statement { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Statement.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        Statement.Write(writer);
        writer.WriteLine(";");
    }
}

internal class ASTSelection : ASTStatement
{
    public required ASTExpression Condition { get; init; }
    public required ASTStatement Then { get; init; }
    public required ASTStatement? Else { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Condition.Visit(visitor);
        Then.Visit(visitor);
        Else?.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("if (");
        Condition.Write(writer);
        writer.WriteLine(")");
        Then.WriteAsNewScope(writer);
        if (Else != null)
        {
            writer.WriteLine("else");
            Else.WriteAsNewScope(writer);
        }
    }
}

internal class ASTSwitchStatement : ASTStatement
{
    public required ASTExpression Value { get; init; }
    public required ASTStatement[] Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Value.Visit(visitor);
        foreach (var body in Body)
            body.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("switch (");
        Value.Write(writer);
        writer.WriteLine(")");
        writer.WriteLine("{");
        using var indented = writer.Indented;
        foreach (var body in Body)
            body.Write(body is ASTCaseLabel or ASTDefaultLabel ? writer : indented);
        writer.WriteLine("}");
    }
}

internal class ASTCaseLabel : ASTStatement
{
    public required ASTExpression Value { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Value.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("case ");
        Value.WriteBracketed(writer,
            Value is not (ASTIntegerLiteral or ASTRealLiteral or ASTVariable));
        writer.WriteLine(":");
    }
}

internal class ASTDefaultLabel : ASTStatement
{
    public override void Visit(IASTVisitor visitor) => visitor.Visit(this);
    public override void Write(CodeWriter writer) => writer.WriteLine("default:");
}

internal class ASTForLoop : ASTStatement
{
    public required ASTStatement Init { get; init; }
    public required ASTExpression Condition { get; init; }
    public required ASTExpression? Update { get; init; }
    public required ASTStatement Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Init.Visit(visitor);
        Condition.Visit(visitor);
        Update?.Visit(visitor);
        Body.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("for (");
        Init.Write(writer); // TODO: Fix new-line in for loops
        writer.Write(' ');
        Condition.Write(writer);
        writer.Write(';');
        if (Update != null)
        {
            writer.Write(' ');
            Update.Write(writer);
        }
        writer.WriteLine(')');
        Body.WriteAsNewScope(writer);
    }
}

internal class ASTWhileLoop : ASTStatement
{
    public required ASTExpression Condition { get; init; }
    public required ASTStatement Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Condition.Visit(visitor);
        Body.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("while (");
        Condition.Write(writer);
        writer.WriteLine(")");
        Body.WriteAsNewScope(writer);
    }
}

internal class ASTDoWhileLoop : ASTWhileLoop
{
    public override void Write(CodeWriter writer)
    {
        writer.WriteLine("do");
        Body.WriteAsNewScope(writer);
        writer.Write("while (");
        Condition.Write(writer);
        writer.WriteLine(");");
    }
}

internal class ASTReturn : ASTStatement
{
    public required ASTExpression? Value { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        Value?.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        writer.Write("return");
        if (Value != null)
        {
            writer.Write(' ');
            Value.Write(writer);
        }
        writer.WriteLine(";");
    }
}

internal class ASTFlowStatement : ASTStatement
{
    public required TokenKind Instruction { get; init; }

    public override void Write(CodeWriter writer) => writer.WriteLine(Instruction switch
    {
        TokenKind.KwBreak => "break;",
        TokenKind.KwContinue => "continue;",
        TokenKind.KwDiscard => "discard;",
        _ => throw new InvalidOperationException("Invalid instruction in FlowStatement")
    });
}

internal class ASTStatementScope : ASTStatement
{
    public required ASTStatement[] Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        visitor.Visit(this);
        foreach (var body in Body)
            body.Visit(visitor);
    }

    public override void Write(CodeWriter writer)
    {
        if (Body.None())
        {
            writer.WriteLine("{ }");
            return;
        }

        writer.WriteLine("{");
        using var indented = writer.Indented;
        foreach (var body in Body)
            body.Write(indented);
        writer.WriteLine("}");
    }

    internal override void WriteAsNewScope(CodeWriter writer) => Write(writer);
}
