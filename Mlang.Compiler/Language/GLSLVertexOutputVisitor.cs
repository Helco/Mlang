using System;
using System.Collections.Generic;
using System.IO;
using Mlang.Model;

namespace Mlang.Language;

internal class GLSLVertexOutputVisitor : GLSLOutputVisitor
{
    public GLSLVertexOutputVisitor(
        ASTStageBlock stageBlock,
        TextWriter outputWriter,
        GLSLOutputContext context) :
        base(stageBlock, outputWriter, context)
    {
    }

    public override bool Visit(ASTNode node) => node switch
    {
        ASTTranslationUnit unit => Write(unit),
        ASTStorageBlock block => Write(block),
        _ => base.Visit(node)
    };

    private bool Write(ASTTranslationUnit unit)
    {
        WritePreamble();
        WriteApplicableStorageBlocks(unit);
        WriteFunctions(unit);
        WriteMainFunction(withInstanceVarTransfers: true);
        return false;
    }

    private bool Write(ASTStorageBlock block)
    {
        switch(block.StorageKind)
        {
            case TokenKind.KwAttributes:
                WriteLocationStorageBlock(block, "in");
                break;
            case TokenKind.KwVarying:
                WriteLocationStorageBlock(block, "out");
                break;
            case TokenKind.KwUniform:
                WriteUniformBlock(block);
                break;
            case TokenKind.KwInstances when IsInstanced:
                WriteLocationStorageBlock(block, "in");
                WriteInstanceVarying(block, "out", withNamePrefix: true);
                break;
            case TokenKind.KwInstances when !IsInstanced:
                WriteUniformBlock(block);
                break;
        }
        return false;
    }
}
