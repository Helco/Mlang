using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal class ASTEmptyStatement : ASTStatement
{
}

internal class ASTDeclarationStatement : ASTStatement
{
    public ASTType Type => Declarations.First().Type;
    public required ASTDeclaration[] Declarations { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Type.Visit(visitor);
        foreach (var declaration in Declarations.Where(t => t.Initializer != null))
            declaration.Initializer!.Visit(visitor);
    }
}

internal class ASTExpressionStatement : ASTStatement
{
    public required ASTExpression Expression { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Expression.Visit(visitor);
    }
}

internal class ASTSelection : ASTStatement
{
    public required ASTExpression Condition { get; init; }
    public required ASTStatement Then { get; init; }
    public required ASTStatement? Else { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Condition.Visit(visitor);
        Then.Visit(visitor);
        Else?.Visit(visitor);
    }
}

internal class ASTSwitchStatement : ASTStatement
{
    public required ASTExpression Value { get; init; }
    public required ASTStatement[] Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Value.Visit(visitor);
        foreach (var body in Body)
            body.Visit(visitor);
    }
}

internal class ASTCaseLabel : ASTStatement
{
    public required ASTExpression Value { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Value.Visit(visitor);
    }
}

internal class ASTDefaultLabel : ASTStatement
{
    public override void Visit(IASTVisitor visitor) => visitor.Visit(this);
}

internal class ASTForLoop : ASTStatement
{
    public required ASTStatement Init { get; init; }
    public required ASTExpression Condition { get; init; }
    public required ASTExpression? Update { get; init; }
    public required ASTStatement Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Init.Visit(visitor);
        Condition.Visit(visitor);
        Update?.Visit(visitor);
        Body.Visit(visitor);
    }
}

internal class ASTWhileLoop : ASTStatement
{
    public required ASTExpression Condition { get; init; }
    public required ASTStatement Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Condition.Visit(visitor);
        Body.Visit(visitor);
    }
}

internal class ASTDoWhileLoop : ASTStatement
{
    // code duplication as it is not much and prevents improper inheritance (a DoWhileLoop is not a WhileLoop)
    public required ASTExpression Condition { get; init; }
    public required ASTStatement Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Condition.Visit(visitor);
        Body.Visit(visitor);
    }
}

internal class ASTReturn : ASTStatement
{
    public required ASTExpression? Value { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Value?.Visit(visitor);
    }
}

internal class ASTFlowStatement : ASTStatement
{
    public required TokenKind Instruction { get; init; }
}

internal class ASTStatementScope : ASTStatement
{
    public required ASTStatement[] Body { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        foreach (var body in Body)
            body.Visit(visitor);
    }
}
