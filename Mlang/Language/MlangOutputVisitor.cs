using System;
using System.Collections.Generic;
using System.Linq;
using Mlang.Model;

namespace Mlang.Language;

internal class MlangOutputVisitor : IASTVisitor
{
    private readonly Stack<CodeWriter> writerStack = new();
    private CodeWriter writer => writerStack.Peek();

    public MlangOutputVisitor(CodeWriter writer)
    {
        writerStack.Push(writer);
    }

    public bool Visit(ASTNode node) => node switch
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
            writer.Write('(');
        expr.Visit(this);
        if (bracketed)
            writer.Write(')');
    }

    private bool Write(ASTIntegerLiteral literal)
    {
        writer.Write(literal.Value);
        return true;
    }

    private bool Write(ASTRealLiteral literal)
    {
        writer.Write(literal.Value);
        writer.Write('f');
        return true;
    }

    private bool Write(ASTVariable variable)
    {
        writer.Write(variable.Name);
        return true;
    }

    private bool Write(ASTFunctionCall call)
    {
        call.Function.Visit(this);
        writer.Write('(');
        if (call.Parameters.Any())
            call.Parameters.First().Visit(this);
        foreach (var param in call.Parameters.Skip(1))
        {
            writer.Write(", ");
            param.Visit(this);
        }
        writer.Write(')');
        return false;
    }

    private bool Write(ASTArrayAccess arr)
    {
        arr.Array.Visit(this);
        writer.Write('[');
        arr.Index.Visit(this);
        writer.Write(']');
        return false;
    }

    private bool Write(ASTMemberAccess mem)
    {
        mem.Parent.Visit(this);
        writer.Write('.');
        writer.Write(mem.Member);
        return false;
    }

    private bool Write(ASTPostUnaryExpression un)
    {
        un.Operand.Visit(this);
        writer.Write(un.Operator switch
        {
            TokenKind.Increment => "++",
            TokenKind.Decrement => "--",
            _ => throw new InvalidOperationException("Invalid operator in PostUnaryExpression")
        });
        return false;
    }

    private bool Write(ASTUnaryExpression un)
    {
        writer.Write(un.Operator switch
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
            writer.Write(' ');
        writer.Write(bin.Operator switch
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
        writer.Write(' ');

        WriteBracketed(bin.Right, bin.Precedence <= bin.Right.Precedence);
        return false;
    }

    private bool Write(ASTConditional cond)
    {
        cond.Condition.Visit(this);
        writer.Write(" ? ");
        cond.Then.Visit(this);
        writer.Write(" : ");
        cond.Else.Visit(this);
        return false;
    }
    #endregion

    #region Statements
    private void PushIndent() => writerStack.Push(writer.Indented);
    private void PopIndent() => writerStack.Pop().Dispose();

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
        writer.WriteLine(';');
        return true;
    }

    private bool Write(ASTDeclarationStatement stmt)
    {
        stmt.Type.Visit(this);
        writer.Write(' ');
        if (stmt.Declarations.Any())
            WriteWithoutType(stmt.Declarations.First());
        foreach (var declaration in stmt.Declarations.Skip(1))
        {
            writer.Write(", ");
            WriteWithoutType(declaration);
        }
        writer.WriteLine(";");
        return false;
    }

    private bool Write(ASTExpressionStatement stmt)
    {
        stmt.Expression.Visit(this);
        writer.WriteLine(';');
        return false;
    }

    private bool Write(ASTSelection sel)
    {
        writer.Write("if (");
        sel.Condition.Visit(this);
        writer.WriteLine(")");
        WriteAsNewScope(sel.Then);
        if (sel.Else != null)
        {
            writer.WriteLine("else");
            WriteAsNewScope(sel.Else);
        }
        return false;
    }

    private bool Write(ASTSwitchStatement stmt)
    {
        writer.Write("switch (");
        stmt.Value.Visit(this);
        writer.WriteLine(")");
        writer.WriteLine("{");
        foreach (var body in stmt.Body)
            WriteAsNewScope(body);
        writer.WriteLine("}");
        return false;
    }

    private bool Write(ASTCaseLabel label)
    {
        writer.Write("case ");
        WriteBracketed(label.Value, label.Value is not (ASTIntegerLiteral or ASTRealLiteral or ASTVariable));
        writer.WriteLine(":");
        return false;
    }

    private bool Write(ASTDefaultLabel _)
    {
        writer.WriteLine("default:");
        return true;
    }

    private bool Write(ASTForLoop loop)
    {
        writer.Write("for (");
        loop.Init.Visit(this); // TODO: Fix new-line in for loops
        writer.Write(' ');
        loop.Condition.Visit(this);
        writer.Write(';');
        if (loop.Update != null)
        {
            writer.Write(' ');
            loop.Update.Visit(this);
        }
        writer.WriteLine(')');
        WriteAsNewScope(loop.Body);
        return false;
    }

    private bool Write(ASTWhileLoop loop)
    {
        writer.Write("while (");
        loop.Condition.Visit(this);
        writer.WriteLine(")");
        WriteAsNewScope(loop.Body);
        return false;
    }

    private bool Write(ASTDoWhileLoop loop)
    {
        writer.WriteLine("do");
        WriteAsNewScope(loop.Body);
        writer.Write("while (");
        loop.Condition.Visit(this);
        writer.WriteLine(");");
        return false;
    }

    private bool Write(ASTReturn ret)
    {
        writer.Write("return");
        if (ret.Value != null)
        {
            writer.Write(' ');
            ret.Value.Visit(this);
        }
        writer.WriteLine(";");
        return false;
    }

    private bool Write(ASTFlowStatement stmt)
    {
        writer.WriteLine(stmt.Instruction switch
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
            writer.WriteLine("{ }");
            return false;
        }

        writer.WriteLine("{");
        PushIndent();
        foreach (var body in scope.Body)
            body.Visit(this);
        PopIndent();
        writer.WriteLine("}");
        return false;
    }
    #endregion

    #region Declarations
    private bool Write(ASTNumericType type)
    {
        writer.Write(type.Type.MlangName);
        return false;
    }

    private bool Write(ASTImageType type)
    {
        writer.Write(type.Type.ToString());
        return false;
    }

    private bool Write(ASTSamplerType type)
    {
        writer.Write(type.Type.AsGLSLName());
        return false;
    }

    private bool Write(ASTCustomType type)
    {
        writer.Write(type.Type);
        return false;
    }

    private bool Write(ASTBufferType _)
    {
        writer.Write("buffer ");
        return true;
    }

    private bool Write(ASTArrayType type)
    {
        type.Element.Visit(this);
        writer.Write('[');
        type.Size?.Visit(this);
        writer.Write(']');
        return false;
    }

    private void WriteWithoutType(ASTDeclaration decl)
    {
        writer.Write(decl.Name);
        if (decl.Initializer != null)
        {
            writer.Write(" = ");
            decl.Initializer.Visit(this);
        }
    }

    private bool Write(ASTDeclaration decl)
    {
        decl.Type.Visit(this);
        writer.Write(' ');
        WriteWithoutType(decl);
        return false;
    }

    private bool Write(ASTFunction func)
    {
        if (func.ReturnType == null)
            writer.Write("void");
        else
            func.ReturnType.Visit(this);
        writer.Write(' ');
        writer.Write(func.Name);
        writer.Write('(');

        if (func.Parameters.Any())
            func.Parameters.First().Visit(this);
        foreach (var param in func.Parameters.Skip(1))
        {
            writer.Write(", ");
            param.Visit(this);
        }
        writer.Write(')');

        if (func.Body == null)
            writer.WriteLine(";");
        else
        {
            writer.WriteLine();
            WriteAsNewScope(func.Body);
        }
        writer.WriteLine();
        return false;
    }

    private bool Write(ASTStorageBlock block)
    {
        writer.Write(block.StorageKind switch
        {
            TokenKind.KwAttributes => "attributes",
            TokenKind.KwInstances => "instances",
            TokenKind.KwUniform => "uniform",
            TokenKind.KwVarying => "varying",
            _ => throw new InvalidOperationException("Invalid storage kind in StorageBlock")
        });
        if (block.Condition != null)
        {
            writer.Write(" if (");
            block.Condition.Visit(this);
            writer.Write(')');
        }

        if (block.Declarations.Length == 1)
        {
            writer.Write(' ');
            block.Declarations.Single().Visit(this);
            writer.WriteLine(';');
        }
        else
        {
            writer.WriteLine();
            writer.WriteLine('{');
            PushIndent();
            foreach (var decl in block.Declarations)
            {
                decl.Visit(this);
                writer.WriteLine(';');
            }
            PopIndent();
            writer.WriteLine('}');
        }
        writer.WriteLine();
        return false;
    }

    private bool Write(ASTStageBlock block)
    {
        writer.WriteLine(block.Stage switch
        {
            TokenKind.KwVertex => "vertex",
            TokenKind.KwFragment => "fragment",
            _ => throw new InvalidOperationException("Invalid stage kind in StageBlock")
        });
        writer.WriteLine("{");
        PushIndent();
        foreach (var func in block.Functions)
        {
            func.Visit(this);
            writer.WriteLine();
        }
        foreach (var stmt in block.Statements)
        {
            stmt.Visit(this);
        }
        PopIndent();
        writer.WriteLine("}");
        writer.WriteLine();
        return false;
    }

    private bool Write(ASTPipelineBlock block)
    {
        if (block.Condition == null)
            writer.WriteLine("pipeline");
        else
        {
            writer.Write("pipeline if (");
            block.Condition.Visit(this);
            writer.WriteLine(")");
        }

        writer.WriteLine("{");
        PushIndent();
        // TODO: Implement this
        writer.WriteLine("// Writing pipeline states is not yet implemented");
        PopIndent();
        writer.WriteLine("}");
        writer.WriteLine();
        return false;
    }

    private bool Write(ASTOption option)
    {
        writer.Write("option ");
        writer.Write(option.Name);
        if (option.NamedValues != null)
        {
            writer.Write(" = ");
            writer.Write(option.NamedValues.First());
            foreach (var value in option.NamedValues.Skip(1))
            {
                writer.Write(", ");
                writer.Write(value);
            }
        }
        writer.WriteLine(";");
        writer.WriteLine();
        return true;
    }
    #endregion
}
