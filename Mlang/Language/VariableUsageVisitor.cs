using System.Collections.Generic;

namespace Mlang.Language;

internal class VariableUsageVisitor<TValue> : IASTVisitor
{
    private readonly IReadOnlyDictionary<string, TValue> allVariables;

    // I would use IReadOnlySet if not targetting .NET Standard 2.0 (because of MSBuild v_v)
    public HashSet<string> UsedVariables { get; } = new();

    public VariableUsageVisitor(IReadOnlyDictionary<string, TValue> allVariables)
    {
        this.allVariables = allVariables;
    }

    public bool Visit(ASTNode node)
    {
        if (node is ASTVariable variable && allVariables.ContainsKey(variable.Name))
            UsedVariables.Add(variable.Name);
        return true;
    }
}
