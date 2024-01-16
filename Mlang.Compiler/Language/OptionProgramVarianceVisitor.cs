using System;
using System.Collections.Generic;

namespace Mlang.Language;

internal class OptionProgramVarianceVisitor : IASTVisitor
{
    private readonly Dictionary<string, ASTOption> remainingOptions;

    public OptionProgramVarianceVisitor(Dictionary<string, ASTOption> remainingOptions)
    {
        this.remainingOptions = remainingOptions;
    }

    public bool Visit(ASTNode node)
    {
        if (remainingOptions.None() ||
            node is ASTPipelineBlock or ASTOption)
            return false;
        // we now already skip all places where option usage does not affect shader programs
        // (which are only pipeline block conditions)

        if (node is ASTVariable variable)
            remainingOptions.Remove(variable.Name);
        if (node is ASTStorageBlock { StorageKind: TokenKind.KwInstances })
            remainingOptions.Remove(ShaderCompiler.IsInstancedOptionName);
        return true;
    }
}
