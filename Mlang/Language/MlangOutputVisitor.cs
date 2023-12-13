using System;
using System.Collections.Generic;
using System.Linq;
using Mlang.Model;

namespace Mlang.Language;

internal class MlangOutputVisitor : IASTVisitor
{
    private readonly Stack<CodeWriter> writerStack = new();
    public CodeWriter Writer => writerStack.Peek();

    public MlangOutputVisitor(CodeWriter writer)
    {
        writerStack.Push(writer);
    }

    public virtual bool Visit(ASTNode node) => node switch
    {
        ASTIntegerLiteral literal => Write(literal),
        ASTRealLiteral literal => Write(literal),
        ASTVariable variable => Write(variable),
        ASTFunctionCall call => Write(call),
        ASTArrayAccess array => Write(array),
        ASTMemberAccess member => Write(member),
        ASTPostUnaryExpression postUnary => Write(postUnary),
        ASTUnaryExpression unary => Write(unary),
        ASTBinaryExpression binary => Write(binary),
        ASTConditional cond => Write(cond),

        ASTEmptyStatement empty => Write(empty),
        ASTDeclarationStatement declStmt => Write(declStmt),
        ASTExpressionStatement exprStmt => Write(exprStmt),
        ASTSelection selection => Write(selection),
        ASTSwitchStatement switchStmt => Write(switchStmt),
        ASTCaseLabel caseLabel => Write(caseLabel),
        ASTDefaultLabel defaultLabel => Write(defaultLabel),
        ASTForLoop forLoop => Write(forLoop),
        ASTWhileLoop whileLoop => Write(whileLoop),
        ASTDoWhileLoop doWhileLoop => Write(doWhileLoop),
        ASTReturn returnStmt => Write(returnStmt),
        ASTFlowStatement flowStmt => Write(flowStmt),
        ASTStatementScope scope => Write(scope),

        ASTNumericType numeric => Write(numeric),
        ASTImageType image => Write(image),
        ASTSamplerType sampler => Write(sampler),
        ASTCustomType custom => Write(custom),
        ASTBufferType buffer => Write(buffer),
        ASTArrayType array => Write(array),
        ASTDeclaration decl => Write(decl),
        ASTFunction func => Write(func),
        ASTStorageBlock storage => Write(storage),
        ASTStageBlock stage => Write(stage),
        ASTPipelineBlock pipeline => Write(pipeline),
        ASTOption option => Write(option),

        ASTTranslationUnit => true,
        _ => throw new NotImplementedException($"Unimplemented node for output: {node?.GetType().Name}")
    };

    #region Expressions
    private void WriteBracketed(ASTExpression expr, bool bracketed)
    {
        if (bracketed)
            Writer.Write('(');
        expr.Visit(this);
        if (bracketed)
            Writer.Write(')');
    }

    private bool Write(ASTIntegerLiteral literal)
    {
        Writer.Write(literal.Value);
        return true;
    }

    private bool Write(ASTRealLiteral literal)
    {
        Writer.Write(literal.Value);
        Writer.Write('f');
        return true;
    }

    private bool Write(ASTVariable variable)
    {
        Writer.Write(variable.Name);
        return true;
    }

    private bool Write(ASTFunctionCall call)
    {
        call.Function.Visit(this);
        Writer.Write('(');
        if (call.Parameters.Any())
            call.Parameters.First().Visit(this);
        foreach (var param in call.Parameters.Skip(1))
        {
            Writer.Write(", ");
            param.Visit(this);
        }
        Writer.Write(')');
        return false;
    }

    private bool Write(ASTArrayAccess arr)
    {
        arr.Array.Visit(this);
        Writer.Write('[');
        arr.Index.Visit(this);
        Writer.Write(']');
        return false;
    }

    private bool Write(ASTMemberAccess mem)
    {
        mem.Parent.Visit(this);
        Writer.Write('.');
        Writer.Write(mem.Member);
        return false;
    }

    private bool Write(ASTPostUnaryExpression un)
    {
        un.Operand.Visit(this);
        Writer.Write(un.Operator switch
        {
            TokenKind.Increment => "++",
            TokenKind.Decrement => "--",
            _ => throw new InvalidOperationException("Invalid operator in PostUnaryExpression")
        });
        return false;
    }

    private bool Write(ASTUnaryExpression un)
    {
        Writer.Write(un.Operator switch
        {
            TokenKind.Increment => "++",
            TokenKind.Decrement => "--",
            TokenKind.Add => "+",
            TokenKind.Subtract => "-",
            TokenKind.Ampersand => "!",
            TokenKind.BitNegate => "~",
            _ => throw new InvalidOperationException("Invalid operator in UnaryExpression")
        });
        return true;
    }

    private bool Write(ASTBinaryExpression bin)
    {
        WriteBracketed(bin.Left, bin.Precedence < bin.Left.Precedence);

        if (bin.Operator != TokenKind.Comma)
            Writer.Write(' ');
        Writer.Write(bin.Operator switch
        {
            TokenKind.Multiplicate => "*",
            TokenKind.Divide => "/",
            TokenKind.Modulo => "%",
            TokenKind.Add => "+",
            TokenKind.Subtract => "-",
            TokenKind.BitshiftL => "<<",
            TokenKind.BitshiftR => ">>",
            TokenKind.Lesser => "<",
            TokenKind.Greater => ">",
            TokenKind.LessOrEquals => "<=",
            TokenKind.GreaterOrEquals => ">=",
            TokenKind.Equals => "==",
            TokenKind.NotEquals => "!=",
            TokenKind.BitAnd => "&",
            TokenKind.BitXor => "^",
            TokenKind.BitOr => "|",
            TokenKind.LogicalAnd => "&&",
            TokenKind.LogicalXor => "^^",
            TokenKind.LogicalOr => "||",
            TokenKind.Assign => "=",
            TokenKind.AddAssign => "+=",
            TokenKind.SubtractAssign => "-=",
            TokenKind.MultiplicateAssign => "*=",
            TokenKind.DivideAssign => "/=",
            TokenKind.ModuloAssign => "%=",
            TokenKind.Comma => ",",
            _ => throw new InvalidOperationException("Invalid operator in BinaryExpression")
        });
        Writer.Write(' ');

        WriteBracketed(bin.Right, bin.Precedence <= bin.Right.Precedence);
        return false;
    }

    private bool Write(ASTConditional cond)
    {
        cond.Condition.Visit(this);
        Writer.Write(" ? ");
        cond.Then.Visit(this);
        Writer.Write(" : ");
        cond.Else.Visit(this);
        return false;
    }
    #endregion

    #region Statements
    protected void PushIndent() => writerStack.Push(Writer.Indented);
    protected void PopIndent() => writerStack.Pop().Dispose();

    private void WriteAsNewScope(ASTStatement stmt)
    {
        if (stmt is ASTStatementScope or ASTCaseLabel or ASTDefaultLabel)
            stmt.Visit(this);
        else
        {
            PushIndent();
            stmt.Visit(this);
            PopIndent();
        }
    }

    private bool Write(ASTEmptyStatement _)
    {
        Writer.WriteLine(';');
        return true;
    }

    private bool Write(ASTDeclarationStatement stmt)
    {
        stmt.Type.Visit(this);
        Writer.Write(' ');
        if (stmt.Declarations.Any())
            WriteWithoutType(stmt.Declarations.First());
        foreach (var declaration in stmt.Declarations.Skip(1))
        {
            Writer.Write(", ");
            WriteWithoutType(declaration);
        }
        Writer.WriteLine(";");
        return false;
    }

    private bool Write(ASTExpressionStatement stmt)
    {
        stmt.Expression.Visit(this);
        Writer.WriteLine(';');
        return false;
    }

    private bool Write(ASTSelection sel)
    {
        Writer.Write("if (");
        sel.Condition.Visit(this);
        Writer.WriteLine(")");
        WriteAsNewScope(sel.Then);
        if (sel.Else != null)
        {
            Writer.WriteLine("else");
            WriteAsNewScope(sel.Else);
        }
        return false;
    }

    private bool Write(ASTSwitchStatement stmt)
    {
        Writer.Write("switch (");
        stmt.Value.Visit(this);
        Writer.WriteLine(")");
        Writer.WriteLine("{");
        foreach (var body in stmt.Body)
            WriteAsNewScope(body);
        Writer.WriteLine("}");
        return false;
    }

    private bool Write(ASTCaseLabel label)
    {
        Writer.Write("case ");
        WriteBracketed(label.Value, label.Value is not (ASTIntegerLiteral or ASTRealLiteral or ASTVariable));
        Writer.WriteLine(":");
        return false;
    }

    private bool Write(ASTDefaultLabel _)
    {
        Writer.WriteLine("default:");
        return true;
    }

    private bool Write(ASTForLoop loop)
    {
        Writer.Write("for (");
        loop.Init.Visit(this); // TODO: Fix new-line in for loops
        Writer.Write(' ');
        loop.Condition.Visit(this);
        Writer.Write(';');
        if (loop.Update != null)
        {
            Writer.Write(' ');
            loop.Update.Visit(this);
        }
        Writer.WriteLine(')');
        WriteAsNewScope(loop.Body);
        return false;
    }

    private bool Write(ASTWhileLoop loop)
    {
        Writer.Write("while (");
        loop.Condition.Visit(this);
        Writer.WriteLine(")");
        WriteAsNewScope(loop.Body);
        return false;
    }

    private bool Write(ASTDoWhileLoop loop)
    {
        Writer.WriteLine("do");
        WriteAsNewScope(loop.Body);
        Writer.Write("while (");
        loop.Condition.Visit(this);
        Writer.WriteLine(");");
        return false;
    }

    private bool Write(ASTReturn ret)
    {
        Writer.Write("return");
        if (ret.Value != null)
        {
            Writer.Write(' ');
            ret.Value.Visit(this);
        }
        Writer.WriteLine(";");
        return false;
    }

    private bool Write(ASTFlowStatement stmt)
    {
        Writer.WriteLine(stmt.Instruction switch
        {
            TokenKind.KwBreak => "break;",
            TokenKind.KwContinue => "continue;",
            TokenKind.KwDiscard => "discard;",
            _ => throw new InvalidOperationException("Invalid instruction in FlowStatement")
        });
        return true;
    }

    private bool Write(ASTStatementScope scope)
    {
        if (scope.Body.None())
        {
            Writer.WriteLine("{ }");
            return false;
        }

        Writer.WriteLine("{");
        PushIndent();
        foreach (var body in scope.Body)
            body.Visit(this);
        PopIndent();
        Writer.WriteLine("}");
        return false;
    }
    #endregion

    #region Declarations
    private bool Write(ASTNumericType type)
    {
        Writer.Write(type.Type.MlangName);
        return false;
    }

    private bool Write(ASTImageType type)
    {
        Writer.Write(type.Type.ToString());
        return false;
    }

    private bool Write(ASTSamplerType type)
    {
        Writer.Write(type.Type.AsGLSLName());
        return false;
    }

    private bool Write(ASTCustomType type)
    {
        Writer.Write(type.Type);
        return false;
    }

    private bool Write(ASTBufferType _)
    {
        Writer.Write("buffer ");
        return true;
    }

    private bool Write(ASTArrayType type)
    {
        type.Element.Visit(this);
        Writer.Write('[');
        type.Size?.Visit(this);
        Writer.Write(']');
        return false;
    }

    private void WriteWithoutType(ASTDeclaration decl)
    {
        Writer.Write(decl.Name);
        if (decl.Initializer != null)
        {
            Writer.Write(" = ");
            decl.Initializer.Visit(this);
        }
    }

    private bool Write(ASTDeclaration decl)
    {
        decl.Type.Visit(this);
        Writer.Write(' ');
        WriteWithoutType(decl);
        return false;
    }

    private bool Write(ASTFunction func)
    {
        if (func.ReturnType == null)
            Writer.Write("void");
        else
            func.ReturnType.Visit(this);
        Writer.Write(' ');
        Writer.Write(func.Name);
        Writer.Write('(');

        if (func.Parameters.Any())
            func.Parameters.First().Visit(this);
        foreach (var param in func.Parameters.Skip(1))
        {
            Writer.Write(", ");
            param.Visit(this);
        }
        Writer.Write(')');

        if (func.Body == null)
            Writer.WriteLine(";");
        else
        {
            Writer.WriteLine();
            WriteAsNewScope(func.Body);
        }
        Writer.WriteLine();
        return false;
    }

    private bool Write(ASTStorageBlock block)
    {
        Writer.Write(block.StorageKind switch
        {
            TokenKind.KwAttributes => "attributes",
            TokenKind.KwInstances => "instances",
            TokenKind.KwUniform => "uniform",
            TokenKind.KwVarying => "varying",
            _ => throw new InvalidOperationException("Invalid storage kind in StorageBlock")
        });
        if (block.Condition != null)
        {
            Writer.Write(" if (");
            block.Condition.Visit(this);
            Writer.Write(')');
        }

        if (block.Declarations.Length == 1)
        {
            Writer.Write(' ');
            block.Declarations.Single().Visit(this);
            Writer.WriteLine(';');
        }
        else
        {
            Writer.WriteLine();
            Writer.WriteLine('{');
            PushIndent();
            foreach (var decl in block.Declarations)
            {
                decl.Visit(this);
                Writer.WriteLine(';');
            }
            PopIndent();
            Writer.WriteLine('}');
        }
        Writer.WriteLine();
        return false;
    }

    private bool Write(ASTStageBlock block)
    {
        Writer.WriteLine(block.Stage switch
        {
            TokenKind.KwVertex => "vertex",
            TokenKind.KwFragment => "fragment",
            _ => throw new InvalidOperationException("Invalid stage kind in StageBlock")
        });
        Writer.WriteLine("{");
        PushIndent();
        foreach (var func in block.Functions)
        {
            func.Visit(this);
            Writer.WriteLine();
        }
        foreach (var stmt in block.Statements)
        {
            stmt.Visit(this);
        }
        PopIndent();
        Writer.WriteLine("}");
        Writer.WriteLine();
        return false;
    }

    private bool Write(ASTPipelineBlock block)
    {
        if (block.Condition == null)
            Writer.WriteLine("pipeline");
        else
        {
            Writer.Write("pipeline if (");
            block.Condition.Visit(this);
            Writer.WriteLine(")");
        }

        Writer.WriteLine("{");
        PushIndent();
        // TODO: Implement this
        Writer.WriteLine("// Writing pipeline states is not yet implemented");
        PopIndent();
        Writer.WriteLine("}");
        Writer.WriteLine();
        return false;
    }

    private bool Write(ASTOption option)
    {
        Writer.Write("option ");
        Writer.Write(option.Name);
        if (option.NamedValues != null)
        {
            Writer.Write(" = ");
            Writer.Write(option.NamedValues.First());
            foreach (var value in option.NamedValues.Skip(1))
            {
                Writer.Write(", ");
                Writer.Write(value);
            }
        }
        Writer.WriteLine(";");
        Writer.WriteLine();
        return true;
    }
    #endregion
}
