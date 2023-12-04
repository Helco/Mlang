using System;
using System.Collections.Generic;
using System.Collections.Generic.Polyfill;
using System.IO;
using System.Linq;
using Mlang.Model;
using static Mlang.Diagnostics;

namespace Mlang.Language;

partial class Compiler
{
    private const string TransferredInstancePrefix = "__instance_";

    private void WriteGLSL(
        PipelineState pipeline,
        TextWriter vertexTextWriter,
        TextWriter fragmentTextWriter,
        IOptionValueSet optionValues)
    {
        if (unit == null)
            return;
        var isInstanced = optionValues.TryGetValue(IsInstancedOptionName, out var value) ? value != 0 : false;

        // Find applicable stage block
        var vertexStageBlock = FindStageBlock(TokenKind.KwVertex, optionValues);
        var fragmentStageBlock = FindStageBlock(TokenKind.KwFragment, optionValues);
        if (vertexStageBlock == null || fragmentStageBlock == null)
            return;

        var transferredInstanceVars = isInstanced
            ? FindUsedInstanceVariables(fragmentStageBlock, optionValues)
            : new();

        // Write preamble
        using var vertexWriter = new CodeWriter(vertexTextWriter, disposeWriter: false);
        using var fragmentWriter = new CodeWriter(fragmentTextWriter, disposeWriter: false);
        WriteGLSLPreamble(vertexWriter);
        WriteGLSLPreamble(fragmentWriter);

        // Write storage blocks
        foreach (var block in unit.Blocks.OfType<ASTStorageBlock>().Where(EvaluateCondition))
        {
            switch(block.StorageKind)
            {
                case TokenKind.KwAttributes:
                    WriteGLSLAttributes(vertexWriter, block);
                    break;
                case TokenKind.KwVarying:
                    WriteGLSLVarying(vertexWriter, block, isOutput: true);
                    WriteGLSLVarying(fragmentWriter, block, isOutput: false);
                    break;
                case TokenKind.KwUniform:
                    WriteGLSLUniform(vertexWriter, block);
                    WriteGLSLUniform(fragmentWriter, block);
                    break;
                case TokenKind.KwInstances when isInstanced:
                    WriteGLSLAttributes(vertexWriter, block);
                    WriteGLSLInstanceVarying(vertexWriter, block, transferredInstanceVars, isOutput: true);
                    WriteGLSLInstanceVarying(fragmentWriter, block, transferredInstanceVars, isOutput: false);
                    break;
                case TokenKind.KwInstances when !isInstanced:
                    WriteGLSLUniform(vertexWriter, block);
                    WriteGLSLUniform(fragmentWriter, block);
                    break;
            }
        }

        foreach (var (name, format) in pipeline.ColorOutputs)
            WriteGLSLColorOutput(fragmentWriter, name, format);

        // Write functions
        foreach (var function in unit.Blocks.OfType<ASTFunction>())
        {
            WriteGLSLFunction(vertexWriter, function);
            WriteGLSLFunction(fragmentWriter, function);
        }
        foreach (var function in vertexStageBlock.Functions)
            WriteGLSLFunction(vertexWriter, function);
        foreach (var function in fragmentStageBlock.Functions)
            WriteGLSLFunction(fragmentWriter, function);

        WriteGLSLMainFunction(vertexWriter, vertexStageBlock, transferredInstanceVars);
        WriteGLSLMainFunction(fragmentWriter, fragmentStageBlock, null);

        bool EvaluateCondition(ASTConditionalGlobalBlock block) =>
            block.EvaluateCondition(optionValues);
    }

    private ASTStageBlock? FindStageBlock(TokenKind stageKind, IOptionValueSet optionValues)
    {
        if (unit == null)
            return null;
        var stageBlocks = unit.Blocks
                .OfType<ASTStageBlock>()
                .Where(b => b.Stage == stageKind && b.EvaluateCondition(optionValues))
                .ToArray() as IEnumerable<ASTStageBlock>;
        if (stageBlocks.None())
        {
            diagnostics.Add(DiagNoStageBlock(stageKind));
            return null;
        }
        if (stageBlocks.Count() > 1)
        {
            var firstBlock = stageBlocks.First();
            var secondBlock = stageBlocks.Skip(1).First();
            diagnostics.Add(DiagMultipleStageBlocks(sourceFile, firstBlock.Range, secondBlock.Range, stageKind));
            return null;
        }
        return stageBlocks.Single();
    }

    private Dictionary<string, ASTDeclaration> FindAllInstanceVariables(IOptionValueSet optionValues)
    {
        var list = unit!.Blocks
            .OfType<ASTStorageBlock>()
            .Where(b => b.StorageKind == TokenKind.KwInstances && b.EvaluateCondition(optionValues))
            .SelectMany(b => b.Declarations);
        var map = new Dictionary<string, ASTDeclaration>();
        foreach (var decl in list)
        {
            if (map.TryGetValue(decl.Name, out var prevDecl))
                diagnostics.Add(DiagDuplicateStorageName(sourceFile, prevDecl, decl));
            else
                map.Add(decl.Name, decl);
        }
        return map;
    }

    private HashSet<ASTDeclaration> FindUsedInstanceVariables(ASTStageBlock fragmentStage, IOptionValueSet optionValues)
    {
        if (unit == null)
            return new();
        var allDecls = FindAllInstanceVariables(optionValues);
        var visitor = new VariableUsageVisitor<ASTDeclaration>(allDecls);

        foreach (var function in unit.Blocks.OfType<ASTFunction>())
            function.Visit(visitor);
        fragmentStage.Visit(visitor);

        return visitor.UsedVariables.Select(name => allDecls[name]).ToHashSet();
    }

    private void WriteGLSLPreamble(CodeWriter writer)
    {
        writer.WriteLine("#version 450");
        writer.WriteLine("#extension GL_KHR_vulkan_glsl: enable");
        writer.WriteLine($"// This file was generated by Mlang {typeof(Compiler).Assembly.GetName().Version}");
        writer.WriteLine();
    }

    private void WriteGLSLAttributes(CodeWriter writer, ASTStorageBlock block) =>
        WriteGLSLStorageBlock(writer, block, "in");

    private void WriteGLSLVarying(CodeWriter writer, ASTStorageBlock block, bool isOutput) =>
        WriteGLSLStorageBlock(writer, block, isOutput ? "out" : "in");

    private void WriteGLSLUniform(CodeWriter writer, ASTStorageBlock block)
    {
        foreach (var decl in block.Declarations.Where(d => d.Type.IsBindingType))
            WriteGLSLDeclaration(writer, decl, "uniform", asStatement: true);
        var nonBindings = block.Declarations.Where(d => !d.Type.IsBindingType);
        if (nonBindings.Any())
        {
            writer.Write("uniform block_");
            writer.Write(block.Range.Start.Line);
            writer.Write('_');
            writer.Write(block.Range.Start.Column);
            writer.WriteLine(" {");
            using var indented = writer.Indented;
            foreach (var decl in nonBindings)
                WriteGLSLDeclaration(indented, decl, prefix: "", asStatement: true);
            writer.WriteLine("}");
        }
        writer.WriteLine();
    }

    private void WriteGLSLInstanceVarying(CodeWriter writer, ASTStorageBlock block, HashSet<ASTDeclaration> usedVariables, bool isOutput)
    {
        foreach (var decl in block.Declarations.Where(usedVariables.Contains))
            WriteGLSLDeclaration(writer, decl,
                prefix: isOutput ? "out" : "in",
                asStatement: true,
                overrideName: isOutput ? TransferredInstancePrefix + decl.Name : decl.Name);
        writer.WriteLine();
    }

    private void WriteGLSLStorageBlock(CodeWriter writer, ASTStorageBlock block, string prefix)
    {
        foreach (var decl in block.Declarations)
            WriteGLSLDeclaration(writer, decl, prefix, asStatement: true);
        writer.WriteLine();
    }

    private void WriteGLSLColorOutput(CodeWriter writer, string name, PixelFormat format)
    {
        if (format.IsDepthOnly()) // TODO: This should be a diagnostic
            throw new NotSupportedException("Depth-only pixel formats cannot be used for color outputs");
        writer.Write("out ");
        writer.Write(format.GetNumericType().GLSLName);
        writer.Write(' ');
        writer.Write(name);
        writer.WriteLine(";");
    }

    private void WriteGLSLDeclaration(CodeWriter writer, ASTDeclaration declaration, string prefix, bool asStatement, string? overrideName = null)
    {
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            writer.Write(prefix);
            writer.Write(' ');
        }
        WriteGLSLType(writer, declaration.Type);
        writer.Write(' ');
        writer.Write(overrideName ?? declaration.Name);
        if (declaration.Type is ASTArrayType array)
        {
            writer.Write('[');
            array.Size?.Write(writer);
            writer.Write(']');
        }
        if (declaration.Type is ASTBufferType)
            writer.Write("; }");
        if (declaration.Initializer != null)
        {
            writer.Write(" = ");
            declaration.Initializer.Write(writer);
        }
        if (asStatement)
            writer.WriteLine(';');
    }

    private void WriteGLSLType(CodeWriter writer, ASTType type)
    {
        switch(type)
        {
            case ASTCustomType custom: writer.Write(custom.Type); break;
            case ASTNumericType numeric: writer.Write(numeric.Type.GLSLName); break;
            case ASTImageType image: writer.Write(image.Type.GLSLName); break;
            case ASTSamplerType sampler: writer.Write(sampler.Type.AsGLSLName()); break;
            case ASTArrayType array: WriteGLSLType(writer, array.Element); break; // yes this is not complete
            case ASTBufferType buffer:
                writer.Write("buffer buffer_");
                writer.Write(buffer.Range.Start.Line);
                writer.Write('_');
                writer.Write(buffer.Range.Start.Column);
                writer.Write(" { ");
                WriteGLSLType(writer, buffer.Inner);
                break;
            default: throw new NotSupportedException("Unsupported GLSL type for generic declaration");
        }
    }

    private void WriteGLSLFunction(CodeWriter writer, ASTFunction function)
    {
        // TODO: This is ignoring the custom types and needs a better implementation of the Mlang Write methods
        function?.Write(writer);
    }

    private void WriteGLSLMainFunction(CodeWriter writer, ASTStageBlock stage, HashSet<ASTDeclaration>? transferredVars)
    {
        writer.WriteLine("void main() {");
        using var indented = writer.Indented;

        if (transferredVars?.Count > 0)
        {
            foreach (var decl in transferredVars)
            {
                indented.Write(TransferredInstancePrefix);
                indented.Write('_');
                indented.Write(decl.Name);
                indented.Write(" = ");
                indented.Write(decl.Name);
                indented.WriteLine(";");
            }
            indented.WriteLine();
        }
        foreach (var stmt in stage.Statements)
            stmt.Write(indented);

        writer.WriteLine("}");
        writer.WriteLine();
    }
}
