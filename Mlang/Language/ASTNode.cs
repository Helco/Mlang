using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Text;
using TextRange = Yoakke.SynKit.Text.Range;

namespace Mlang.Language;

internal interface IASTVisitor
{
    bool Visit(ASTNode node);
}

internal abstract class ASTNode
{
    public required TextRange Range { get; init; }

    public virtual void Visit(IASTVisitor visitor) => visitor.Visit(this);
}

internal abstract class ASTExpression : ASTNode
{
    public virtual int Precedence { get; } = -1000;

    public virtual bool TryOptionEvaluate(IOptionValueSet optionValues, out long value)
    {
        value = 0;
        return false;
    }
}

internal abstract class ASTStatement : ASTNode
{
}

internal abstract class ASTType : ASTNode
{
    public abstract bool IsBindingType { get; }
}

internal abstract class ASTGlobalBlock : ASTNode
{
}

internal abstract class ASTConditionalGlobalBlock : ASTGlobalBlock
{
    public required ASTExpression? Condition { get; init; }

    public bool EvaluateCondition(IOptionValueSet optionValues)
    {
        if (Condition == null)
            return true;
        if (!Condition.TryOptionEvaluate(optionValues!, out var value))
            throw new InvalidOperationException("Condition was not evaluable, semantic analysis failed to catch this");
        return value != 0;
    }
}

internal class ASTTranslationUnit : ASTNode
{
    public required ASTGlobalBlock[] Blocks { get; init; }

    public override void Visit(IASTVisitor visitor)
    {
        if (!visitor.Visit(this))
            return;
        foreach (var block in Blocks)
            block.Visit(visitor);
    }
}
