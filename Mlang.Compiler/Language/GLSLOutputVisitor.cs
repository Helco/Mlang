﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mlang.Model;

namespace Mlang.Language;

internal abstract class GLSLOutputVisitor : MlangOutputVisitor
{
    protected const string TransferredInstancePrefix = Compiler.TransferredInstancePrefix;

    protected readonly ASTStageBlock stageBlock;
    protected readonly PipelineState pipeline;
    protected readonly IOptionValueSet optionValues;
    protected readonly ISet<ASTDeclaration> transferredInstanceVars;
    protected bool IsInstanced => optionValues.GetBool(Compiler.IsInstancedOptionName);

    protected GLSLOutputVisitor(
        ASTStageBlock stageBlock,
        PipelineState pipeline,
        IOptionValueSet optionValues,
        ISet<ASTDeclaration> transferredInstanceVars,
        TextWriter outputWriter)
        : base(new CodeWriter(outputWriter, disposeWriter: false))
    {
        this.stageBlock = stageBlock;
        this.pipeline = pipeline;
        this.optionValues = optionValues;
        this.transferredInstanceVars = transferredInstanceVars;
    }

    public override bool Visit(ASTNode node) => node switch
    {
        ASTNumericType numeric => Write(numeric),
        ASTArrayType array => Write(array),
        ASTBufferType buffer => Write(buffer),
        ASTSelection selection => Write(selection),

        _ => base.Visit(node)
    };

    protected void WritePreamble()
    {
        Writer.WriteLine("#version 450");
        Writer.WriteLine("#extension GL_EXT_scalar_block_layout : require");
        Writer.WriteLine($"// This file was generated by Mlang {typeof(Compiler).Assembly.GetName().Version}");
        Writer.WriteLine();
    }

    protected void WriteApplicableStorageBlocks(ASTTranslationUnit unit)
    {
        foreach (var block in unit.Blocks.OfType<ASTStorageBlock>().Where(b => b.EvaluateCondition(optionValues)))
            block.Visit(this);
    }

    protected void WriteFunctions(ASTTranslationUnit unit)
    {
        foreach (var function in unit.Blocks.OfType<ASTFunction>().Concat(stageBlock.Functions))
            function.Visit(this);
    }

    protected void WriteLocationStorageBlock(ASTStorageBlock block, string prefix)
    {
        foreach (var decl in block.Declarations)
        {
            WriteLocationLayout(decl);
            WriteDeclaration(decl, prefix, asStatement: true);
        }
        Writer.WriteLine();
    }

    protected void WriteInstanceVarying(ASTStorageBlock block, string prefix)
    {
        foreach (var decl in block.Declarations.Where(transferredInstanceVars.Contains))
        {
            WriteLocationLayout(decl, useOutLocation: true);
            WriteDeclaration(decl, prefix, asStatement: true, TransferredInstancePrefix);
        }
        Writer.WriteLine();
    }

    protected void WriteUniformBlock(ASTStorageBlock block)
    {
        foreach (var decl in block.Declarations.Where(d => d.Type.IsBindingType))
        {
            WriteBindingLayout(decl);
            WriteDeclaration(decl, "uniform", asStatement: true);
        }
        var nonBindings = block.Declarations.Where(d => !d.Type.IsBindingType);
        if (nonBindings.Any())
        {
            WriteBindingLayout(nonBindings.First(), forceStd430: true);
            Writer.Write("uniform ");
            Writer.Write(block.NameForGLSL);
            Writer.WriteLine(" {");
            PushIndent();
            foreach (var decl in nonBindings)
                WriteDeclaration(decl, prefix: "", asStatement: true);
            PopIndent();
            Writer.WriteLine("};");
        }
        Writer.WriteLine();
    }

    private void WriteDeclaration(ASTDeclaration declaration, string prefix, bool asStatement, string? namePrefix = null)
    {
        var mlangVisitor = new MlangOutputVisitor(Writer);
        if (!string.IsNullOrWhiteSpace(prefix) && declaration.Type is not ASTBufferType)
        {
            Writer.Write(prefix);
            Writer.Write(' ');
        }
        declaration.Type.Visit(this);
        Writer.Write(' ');
        if (namePrefix != null)
            Writer.Write(namePrefix);
        Writer.Write(declaration.Name);
        if (declaration.Type is ASTArrayType array)
        {
            Writer.Write('[');
            array.Size?.Visit(mlangVisitor);
            Writer.Write(']');
        }
        if (declaration.Type is ASTBufferType)
            Writer.Write("; }");
        if (declaration.Initializer != null)
        {
            Writer.Write(" = ");
            declaration.Initializer.Visit(mlangVisitor);
        }
        if (asStatement)
            Writer.WriteLine(';');
    }

    protected void WriteMainFunction(bool withInstanceVarTransfers)
    {
        Writer.WriteLine("void main() {");
        PushIndent();

        if (withInstanceVarTransfers && transferredInstanceVars.Count > 0)
        {
            foreach (var decl in transferredInstanceVars)
            {
                Writer.Write(TransferredInstancePrefix);
                Writer.Write(decl.Name);
                Writer.Write(" = ");
                Writer.Write(decl.Name);
                Writer.WriteLine(";");
            }
            Writer.WriteLine();
        }
        foreach (var stmt in stageBlock.Statements)
            stmt.Visit(this);
        PopIndent();

        Writer.WriteLine("}");
        Writer.WriteLine();
    }

    private bool Write(ASTNumericType type)
    {
        Writer.Write(type.Type.GLSLName); // instead of Mlang, e.g. float3 -> vec3
        return false;
    }

    private bool Write(ASTArrayType array)
    {
        // we have to move the size at the end of each declaration
        array.Element.Visit(this);
        return false;
    }

    private bool Write(ASTBufferType buffer)
    {
        Writer.Write("buffer buffer_");
        Writer.Write(buffer.Range.Start.Line);
        Writer.Write('_');
        Writer.Write(buffer.Range.Start.Column);
        Writer.Write(" { ");
        buffer.Inner.Visit(this);
        return false;
    }

    private bool Write(ASTSelection selection)
    {
        if (!selection.Condition.TryOptionEvaluate(optionValues, out var value))
            return base.Visit(selection);
        if (value == 0)
            selection.Else?.Visit(this);
        else
            selection.Then.Visit(this);
        return false;
    }

    private void WriteLocationLayout(ASTDeclaration decl, bool useOutLocation = false)
    {
        var location = useOutLocation ? decl.Layout.OutLocation : decl.Layout.InLocation;
        if (location == null)
            return;
        Writer.Write("layout(location = ");
        Writer.Write(location);
        Writer.Write(") ");
    }

    private void WriteBindingLayout(ASTDeclaration decl, bool forceStd430 = false)
    {
        if (decl.Layout.Binding == null || decl.Layout.Set == null)
            return;
        if (decl.Type is ASTBufferType)
            forceStd430 = true;

        Writer.Write("layout(");
        if (forceStd430)
            Writer.Write("std430, ");
        Writer.Write("set = ");
        Writer.Write(decl.Layout.Set);
        Writer.Write(", binding = ");
        Writer.Write(decl.Layout.Binding);
        Writer.Write(") ");
    }
}
