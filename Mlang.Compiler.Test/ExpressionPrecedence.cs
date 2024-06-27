using Mlang.Language;
using System.Globalization;

namespace Mlang.Compiler.Test;

public class ExpressionPrecedence
{
    [TestCase("a.x", "a.x", TestName = "MemberAccessAfterVariable")]
    [TestCase("a.x.y", "a.x.y", TestName = "MemberAccessAfterMemberAccess")]
    [TestCase("a.x().y", "a.x().y", TestName = "MemberAccessAfterMemberAccess2")]
    [TestCase("a().x", "a().x", TestName = "MemberAccessAfterCall")]
    [TestCase("(a + b).x", "(a + b).x", TestName = "MemberAccessAfterBinary")]
    [TestCase("-a", "-a", TestName = "UnaryBeforeVariable")]
    [TestCase("-!a", "-!a", TestName = "UnaryBeforeUnary")]
    [TestCase("-a()", "-a()", TestName = "UnaryBeforeCall")]
    [TestCase("-(a + b)", "-(a + b)", TestName = "UnaryBeforeBinary")]
    [TestCase("a()[0]", "a()[0]", TestName = "ArrayAfterCall")]
    [TestCase("++a[0]", "++a[0]", TestName = "UnaryBeforeArray")]
    [TestCase("--++a[0]", "--++a[0]", TestName = "DoubleUnaryBeforeArray")]
    [TestCase("a[0]++", "a[0]++", TestName = "PostUnaryAfterArray")]
    [TestCase("a[0]++++", "a[0]++++", TestName = "DoublePostUnaryAfterArray")]
    public void TestExpressionOutput(string input, string output)
    {
        var lexer = new Lexer(input);
        var parser = new Parser(lexer) { SourceFile = null! };
        var exprResult = parser.ParseExpression();
        Assert.That(exprResult.IsOk);

        var stringWriter = new StringWriter(CultureInfo.InvariantCulture);
        using var codeWriter = new CodeWriter(stringWriter);
        var mlangOutput = new MlangOutputVisitor(codeWriter);
        mlangOutput.Visit(exprResult.Ok.Value);

        Assert.That(stringWriter.ToString(), Is.EqualTo(output));
    }
}