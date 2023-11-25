using System;
using System.Collections.Generic;
using Yoakke.SynKit.Text;

namespace Mlang.Language;

internal abstract class ASTNode
{
}

internal abstract class ASTExpression : ASTNode
{
}

internal abstract class ASTStatement : ASTNode
{
}

internal abstract class ASTType : ASTNode
{
}

internal abstract class ASTDeclaration : ASTNode
{
}

internal readonly record struct BlendFormula(int a, int b);

internal abstract class ASTGlobalBlock : ASTNode
{
}
