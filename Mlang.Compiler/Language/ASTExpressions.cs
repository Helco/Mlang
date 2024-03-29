using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlang.Language;

internal class ASTIntegerLiteral : ASTExpression
{
    public override int Precedence => -3;
    public long Value { get; init; }

    public override bool TryOptionEvaluate(IOptionValueSet optionValues, out long value)
    {
        value = Value;
        return true;
    }
}

internal class ASTRealLiteral : ASTExpression
{
    public override int Precedence => -3;
    public double Value { get; init; }
}

internal class ASTVariable : ASTExpression
{
    public override int Precedence => -3;
    public required string Name { get; init; }

    public override bool TryOptionEvaluate(IOptionValueSet optionValues, out long value)
    {
        var result = optionValues.TryGetValue(Name, out var intValue);
        value = intValue;
        return result;
    }
}

internal class ASTFunctionCall : ASTExpression
{
    public override int Precedence => -2;
    public required ASTExpression Function { get; init; }
    public required ASTExpression[] Parameters { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Function.Visit(visitor);
        foreach (var param in Parameters)
            param.Visit(visitor);
    }
}

internal class ASTArrayAccess : ASTExpression
{
    public override int Precedence => -2;
    public required ASTExpression Array { get; init; }
    public required ASTExpression Index { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Array.Visit(visitor);
        Index.Visit(visitor);
    }
}

internal class ASTMemberAccess : ASTExpression
{
    public override int Precedence => -2;
    public required ASTExpression Parent { get; init; }
    public required string Member { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Parent.Visit(visitor);
    }
}

internal class ASTPostUnaryExpression : ASTExpression
{
    public override int Precedence => -2;
    public required ASTExpression Operand { get; init; }
    public required TokenKind Operator { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Operand.Visit(visitor);
    }
}

internal class ASTUnaryExpression : ASTExpression
{
    public override int Precedence => -1;
    public required ASTExpression Operand { get; init; }
    public required TokenKind Operator { get; init; }

    public override bool TryOptionEvaluate(IOptionValueSet optionValues, out long value)
    {
        if (!Operand.TryOptionEvaluate(optionValues, out value))
            return false;
        switch (Operator)
        {
            // for options I purposefully do not implement arithemtic operations
            case TokenKind.Ampersand: value = value == 0 ? 1 : 0; return true;
            default: return false;
        }
    }

    public override void Visit(IASTVisitor visitor)
    {
        if (visitor.Visit(this))
            Operand.Visit(visitor);
    }
}

internal class ASTBinaryExpression : ASTExpression
{
    public override int Precedence => Precedences.TryGetValue(Operator, out int prec) ? prec : -1000;
    public required ASTExpression Left { get; init; }
    public required ASTExpression Right { get; init; }
    public required TokenKind Operator { get; init; }

    public override bool TryOptionEvaluate(IOptionValueSet optionValues, out long value)
    {
        value = 0;
        if (!Left.TryOptionEvaluate(optionValues, out var leftValue) ||
            !Right.TryOptionEvaluate(optionValues, out var rightValue))
            return false;
        switch(Operator)
        {
            // for options I purposefully do not implement arithemtic operations
            case TokenKind.Lesser: value = leftValue < rightValue ? 1 : 0; return true;
            case TokenKind.Greater: value = leftValue > rightValue ? 1 : 0; return true;
            case TokenKind.LessOrEquals: value = leftValue <= rightValue ? 1 : 0; return true;
            case TokenKind.GreaterOrEquals: value = leftValue >= rightValue ? 1 : 0; return true;
            case TokenKind.Equals: value = leftValue == rightValue ? 1 : 0; return true;
            case TokenKind.NotEquals: value = leftValue != rightValue ? 1 : 0; return true;
            case TokenKind.LogicalAnd: value = leftValue != 0 && rightValue != 0 ? 1 : 0; return true;
            case TokenKind.LogicalOr: value = leftValue != 0 || rightValue != 0 ? 1 : 0; return true;
            case TokenKind.LogicalXor: value = (leftValue != 0) ^ (rightValue != 0) ? 1 : 0; return true;
            default: return false;
        }
    }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Left.Visit(visitor);
        Right.Visit(visitor);
    }

    private static readonly IReadOnlyDictionary<TokenKind, int> Precedences = new Dictionary<TokenKind, int>()
    {
        { TokenKind.Multiplicate,       0 },
        { TokenKind.Divide,             0 },
        { TokenKind.Modulo,             0 },
        { TokenKind.Add,                1 },
        { TokenKind.Subtract,           1 },
        { TokenKind.BitshiftL,          2 },
        { TokenKind.BitshiftR,          2 },
        { TokenKind.Lesser,             3 },
        { TokenKind.Greater,            3 },
        { TokenKind.LessOrEquals,       3 },
        { TokenKind.GreaterOrEquals,    3 },
        { TokenKind.Equals,             4 },
        { TokenKind.NotEquals,          4 },
        { TokenKind.BitAnd,             5 },
        { TokenKind.BitXor,             6 },
        { TokenKind.BitOr,              7 },
        { TokenKind.LogicalAnd,         8 },
        { TokenKind.LogicalXor,         9 },
        { TokenKind.LogicalOr,          10 },
        { TokenKind.Assign,             11 },
        { TokenKind.AddAssign,          11 },
        { TokenKind.SubtractAssign,     11 },
        { TokenKind.MultiplicateAssign, 11 },
        { TokenKind.DivideAssign,       11 },
        { TokenKind.Comma,              1000 },
    };
}

internal class ASTConditional : ASTExpression
{
    public override int Precedence => 20;
    public required ASTExpression Condition { get; init; }
    public required ASTExpression Then { get; init; }
    public required ASTExpression Else { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        Condition.Visit(visitor);
        Then.Visit(visitor);
        Else.Visit(visitor);
    }
}
