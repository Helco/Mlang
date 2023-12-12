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
    private const string TransferredInstancePrefix = "instance_";

    private void WriteGLSL(
        PipelineState pipeline,
        TextWriter vertexTextWriter,
        TextWriter fragmentTextWriter,
        IOptionValueSet optionValues)
    {
        if (unit == null)
            return;
        var isInstanced = optionValues.TryGetValue(IsInstancedOptionName, out var value) ? value != 0 : false;

        var vertexStageBlock = FindStageBlock(TokenKind.KwVertex, optionValues);
        var fragmentStageBlock = FindStageBlock(TokenKind.KwFragment, optionValues);
        if (vertexStageBlock == null || fragmentStageBlock == null)
            return;

        var transferredInstanceVars = isInstanced
            ? FindUsedInstanceVariables(fragmentStageBlock, optionValues)
            : new();

        var vertexVisitor = new GLSLVertexOutputVisitor(vertexStageBlock, pipeline, optionValues, transferredInstanceVars, vertexTextWriter);
        var fragmentVisitor = new GLSLFragmentOutputVisitor(fragmentStageBlock, pipeline, optionValues, transferredInstanceVars, fragmentTextWriter);
        unit.Visit(vertexVisitor);
        unit.Visit(fragmentVisitor);
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
}
