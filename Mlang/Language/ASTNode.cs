using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Text;
using TextRange = Yoakke.SynKit.Text.Range;

namespace Mlang.Language;

internal interface IASTVisitor
{
    void Visit(ASTNode node);
}

internal interface IOptionValueSet
{
    bool TryGetValue(string name, out uint value);
}

internal abstract class ASTNode
{
    public required TextRange Range { get; init; }

    public virtual void Visit(IASTVisitor visitor) => visitor.Visit(this);

    public abstract void Write(CodeWriter writer);
}

internal abstract class ASTExpression : ASTNode
{
    public virtual bool TryOptionEvaluate(IOptionValueSet optionValues, out long value)
    {
        value = 0;
        return false;
    }
    
    internal void WriteBracketed(CodeWriter writer, bool bracketed)
    {
        if (bracketed)
            writer.Write('(');
        Write(writer);
        if (bracketed)
            writer.Write(')');
    }
}

internal abstract class ASTStatement : ASTNode
{
    internal virtual void WriteAsNewScope(CodeWriter writer) => Write(writer.Indented);
}

internal abstract class ASTType : ASTNode
{
}

internal abstract class ASTGlobalBlock : ASTNode
{
}

internal class ASTOption : ASTGlobalBlock
{
    public required int Index { get; init; }
    public required int BitOffset { get; init; }
    public required string Name { get; init; }
    public required string[]? NamedValues { get; init; }
    public int ValueCount => NamedValues?.Length ?? 2;
    public int BitCount => (int)Math.Ceiling(Math.Log(ValueCount) / Math.Log(2));

    public override void Write(CodeWriter writer)
    {
        writer.Write("option ");
        writer.Write(Name);
        if (NamedValues != null)
        {
            writer.Write(" = ");
            writer.Write(NamedValues.First());
            foreach (var value in NamedValues.Skip(1))
            {
                writer.Write(", ");
                writer.Write(value);
            }
        }
        writer.WriteLine(";");
    }
}
