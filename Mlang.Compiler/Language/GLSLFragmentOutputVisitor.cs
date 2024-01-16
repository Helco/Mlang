using System;
using System.Collections.Generic;
using System.IO;
using Mlang.Model;

namespace Mlang.Language;

internal class GLSLFragmentOutputVisitor : GLSLOutputVisitor
{
    public GLSLFragmentOutputVisitor(
        ASTStageBlock stageBlock,
        PipelineState pipeline,
        IOptionValueSet optionValues,
        ISet<ASTDeclaration> transferredInstanceVars,
        IReadOnlyDictionary<ASTDeclaration, LayoutInfo> layout,
        TextWriter outputWriter) : 
        base(stageBlock, pipeline, optionValues, transferredInstanceVars, layout, outputWriter)
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
        foreach (var (i, (name, format)) in pipeline.ColorOutputs.Indexed())
            WriteColorOutput(i, name, format);
        WriteFunctions(unit);
        WriteMainFunction(withInstanceVarTransfers: false);
        return false;
    }

    private bool Write(ASTStorageBlock block)
    {
        switch(block.StorageKind)
        {
            case TokenKind.KwVarying:
                WriteLocationStorageBlock(block, "in");
                break;
            case TokenKind.KwUniform:
                WriteUniformBlock(block);
                break;
            case TokenKind.KwInstances when IsInstanced:
                WriteLocationStorageBlock(block, "flat in");
                break;
            case TokenKind.KwInstances when !IsInstanced:
                WriteUniformBlock(block);
                break;
        }
        return false;
    }

    private void WriteColorOutput(int index, string name, PixelFormat format)
    {
        if (format.IsDepthOnly()) // TODO: This should be a diagnostic
            throw new NotSupportedException("Depth-only pixel formats cannot be used for color outputs");
        Writer.Write("layout(location = ");
        Writer.Write(index);
        Writer.Write(") out ");
        Writer.Write(format.GetNumericType().GLSLName);
        Writer.Write(' ');
        Writer.Write(name);
        Writer.WriteLine(";");
    }
}
