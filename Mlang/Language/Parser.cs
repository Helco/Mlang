using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Parser;
using Yoakke.SynKit.Parser.Attributes;
using Yoakke.SynKit.Text;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang.Language;

[Parser(typeof(TokenKind))]
internal partial class Parser
{
    private List<Diagnostic> diagnostics = new();

    // TODO: Add polyfill for required instead of hacking null into this
    public ISourceFile SourceFile { get; init; } = null!;
    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;

#region Expressions
    [Rule("PostfixExpression: PrimaryExpression")]
    [Rule("UnaryExpression: PostfixExpression")]
    [Rule("MultiplicativeExpression: UnaryExpression")]
    [Rule("AdditiveExpression: MultiplicativeExpression")]
    [Rule("ShiftExpression: AdditiveExpression")]
    [Rule("RelationalExpression: ShiftExpression")]
    [Rule("EqualityExpression: RelationalExpression")]
    [Rule("BitAndExpression: EqualityExpression")]
    [Rule("BitXorExpression: BitAndExpression")]
    [Rule("BitOrExpression: BitXorExpression")]
    [Rule("LogicalAndExpression: BitOrExpression")]
    [Rule("LogicalXorExpression: LogicalAndExpression")]
    [Rule("LogicalOrExpression: LogicalXorExpression")]
    [Rule("ConditionalExpression: LogicalOrExpression")]
    [Rule("AssignmentExpression: ConditionalExpression")]
    [Rule("Expression: AssignmentExpression")]
    private ASTExpression IdentityExpression(ASTExpression expr) => expr;

    [Rule("PrimaryExpression: ExprBrL Expression ExprBrR")]
    private ASTExpression PrimaryExpression(Tk _0, ASTExpression expr, Tk _1) => expr;

    [Rule("PrimaryExpression: UnsignedInteger")]
    private ASTExpression PrimaryExpressionInt(Tk literal) =>
        throw new NotImplementedException();

    [Rule("PrimaryExpression: UnsignedReal")]
    private ASTExpression PrimaryExpressionReal(Tk literal) =>
        throw new NotImplementedException();
    
    [Rule("PrimaryExpression: Identifier")]
    private ASTExpression PrimaryExpressionVar(Tk varIdentifier) =>
        throw new NotImplementedException();

    [Rule("PostfixExpression: PostfixExpression ExprBrL (Expression (Comma Expression)*)? ExprBrR")]
    private ASTExpression PostfixExpressionCall(ASTExpression function, Tk _0, Punctuated<ASTExpression, Tk> parameters, Tk _1) =>
        throw new NotImplementedException();

    [Rule("PostfixExpression: PostfixExpression ArrayBrL Expression ArrayBrR")]
    private ASTExpression PostfixExpressionArray(ASTExpression array, Tk _0, ASTExpression index, Tk _1) =>
        throw new NotImplementedException();

    [Rule("PostfixExpression: PostfixExpression ( Increment | Decrement )")]
    private ASTExpression PostfixExpressionUnary(ASTExpression array, Tk op) =>
        throw new NotImplementedException();

    [Rule("PostfixExpression: PostfixExpression Dot Identifier")]
    private ASTExpression PostfixExpressionMember(ASTExpression parent, Tk _0, Tk member) =>
        throw new NotImplementedException();

    [Rule("UnaryExpression: ( Increment | Decrement | Add | Subtract | Ampersand | BitNegate ) UnaryExpression")]
    private ASTExpression UnaryExpressionPrefix(Tk op, ASTExpression value) =>
        throw new NotImplementedException();

    [Rule("MultiplicativeExpression: MultiplicativeExpression ( Multiplicate | Divide | Modulo ) UnaryExpression")]
    [Rule("AdditiveExpression: AdditiveExpression ( Add | Subtract ) MultiplicativeExpression")]
    [Rule("ShiftExpression: ShiftExpression ( BitshiftL | BitshiftR ) AdditiveExpression")]
    [Rule("RelationalExpression: RelationalExpression ( Lesser | Greater | LessOrEquals | GreaterOrEquals ) ShiftExpression")]
    [Rule("EqualityExpression: EqualityExpression ( Equals | NotEquals ) RelationalExpression")]
    [Rule("BitAndExpression: BitAndExpression BitAnd EqualityExpression")]
    [Rule("BitXorExpression: BitXorExpression BitXor BitAndExpression")]
    [Rule("BitOrExpression: BitOrExpression BitOr BitXorExpression")]
    [Rule("LogicalAndExpression: LogicalAndExpression LogicalAnd BitOrExpression")]
    [Rule("LogicalXorExpression: LogicalXorExpression LogicalXor LogicalAndExpression")]
    [Rule("LogicalOrExpression: LogicalOrExpression LogicalOr LogicalXorExpression")]
    [Rule("AssignmentExpression: PostfixExpression ( Assign | AddAssign | SubtractAssign | MultiplicateAssign | DivideAssign | ModuloAssign ) AssignmentExpression")]
    [Rule("Expression: Expression Comma AssignmentExpression")]
    private ASTExpression BinaryExpression(ASTExpression left, Tk op, ASTExpression right) =>
        throw new NotImplementedException();

    [Rule("ConditionalExpression: LogicalOrExpression Question Expression Colon AssignmentExpression")]
    private ASTExpression ConditionalExpression(ASTExpression condition, Tk _0, ASTExpression thenValue, Tk _1, ASTExpression elseValue) =>
        throw new NotImplementedException();
#endregion

#region Statements
    [Rule("Statement: EmptyStatement | DeclarationStatement | ExpressionStatement | SelectionStatement | SwitchStatement | ForLoop | WhileLoop | DoWhileLoop | ReturnStatement | FlowStatement | StatementScope")]
    [Rule("SwitchBodyStatement: Statement | CaseLabel | DefaultLabel")]
    [Rule("ForInitStatement: DeclarationStatement | ExpressionStatement")]
    private ASTStatement Statement(ASTStatement stmt) => stmt;

    [Rule("EmptyStatement: Semicolon")]
    private ASTStatement EmptyStatement(Tk _0) =>
        throw new NotImplementedException();

    [Rule("ExpressionStatement: Expression Semicolon")]
    private ASTStatement ExpressionStatement(ASTExpression expr, Tk _0) =>
        throw new NotImplementedException();

    [Rule("SelectionStatement: KwIf ExprBrL Expression ExprBrR Statement ElseStatement?")]
    private ASTStatement SelectionStatement(Tk _0, Tk _1, ASTExpression condition, Tk _2, ASTStatement thenBody, ASTStatement? elseBody) =>
        throw new NotImplementedException();

    [Rule("ElseStatement: KwElse Statement")]
    private ASTStatement ElseStatement(Tk _0, ASTStatement body) => body;

    [Rule("SwitchStatement: KwSwitch ExprBrL Expression ExprBrR ScopeBrL SwitchBodyStatement* ScopeBrR")]
    private ASTStatement SwitchStatement(Tk _0, Tk _1, ASTExpression value, Tk _2, Tk _3, IReadOnlyList<ASTStatement> body, Tk _4) =>
        throw new NotImplementedException();

    [Rule("CaseLabel: KwCase Expression Colon")]
    private ASTStatement CaseLabel(Tk _0, ASTExpression value, Tk _1) =>
        throw new NotImplementedException();

    [Rule("DefaultLabel: KwDefault Colon")]
    private ASTStatement DefaultLabel(Tk _0, Tk _1) =>
        throw new NotImplementedException();

    [Rule("ForLoop: KwFor ExprBrL ForInitStatement Expression Semicolon Expression ExprBrR Statement")]
    private ASTStatement ForLoop(Tk _0, Tk _1, ASTStatement init, ASTExpression condition, Tk _2, ASTExpression update, Tk _3, ASTStatement body) =>
        throw new NotImplementedException();

    [Rule("WhileLoop: KwWhile ExprBrL Expression ExprBrR Statement")]
    private ASTStatement WhileLoop(Tk _0, Tk _1, ASTExpression condition, Tk _2, ASTStatement body) =>
        throw new NotImplementedException();

    [Rule("DoWhileLoop: KwDo Statement KwWhile ExprBrL Expression ExprBrR Semicolon")]
    private ASTStatement DoWhileLoop(Tk _0, ASTStatement body, Tk _1, Tk _2, ASTExpression condition, Tk _3, Tk _4) =>
        throw new NotImplementedException();

    [Rule("ReturnStatement: KwReturn Expression? Semicolon")]
    private ASTStatement ReturnStatement(Tk _0, ASTExpression? value, Tk _1) =>
        throw new NotImplementedException();

    [Rule("FlowStatement: (KwBreak | KwContinue | KwDiscard) Semicolon")]
    private ASTStatement FlowStatement(Tk instruction, Tk _0) =>
        throw new NotImplementedException();

    [Rule("DeclarationStatement: Type (DeclarationWithInitializer (Comma DeclarationWithInitializer)*) Semicolon")]
    private ASTStatement DeclarationStatement(ASTType type, Punctuated<(string name, ASTExpression? value), Tk> declarations, Tk _0) =>
        throw new NotImplementedException();

    [Rule("StatementScope: ScopeBrL Statement* ScopeBrR")]
    private ASTStatement StatementScope(Tk _0, IReadOnlyList<ASTStatement> body, Tk _1) =>
        throw new NotImplementedException();
#endregion

#region Types and declarations
    [Rule("Type: Identifier")]
    private ASTType NamedType(Tk name) =>
        throw new NotImplementedException();

    [Rule("Type: KwBuffer Type")]
    private ASTType QualifiedType(Tk qualifier, ASTType type) =>
        throw new NotImplementedException();

    [Rule("Type: Type ArrayBrL Expression? ArrayBrR")]
    private ASTType ArrayType(ASTType element, Tk _0, ASTExpression? size, Tk _1) =>
        throw new NotImplementedException();

    [Rule("DeclarationWithInitializer: Identifier DeclarationInitializer?")]
    private (string name, ASTExpression? value) DeclarationWithInitializer(Tk name, ASTExpression? value) =>
        (name.Text, value);

    [Rule("DeclarationInitializer: Assign Expression")]
    private ASTExpression DeclarationInitializer(Tk _0, ASTExpression value) => value;

    [Rule("SingleFullDeclaration: Type Identifier DeclarationInitializer?")]
    private ASTDeclaration SingleFullDeclaration(ASTType type, Tk name, ASTExpression? value) =>
        throw new NotImplementedException();
#endregion

#region Functions
    [Rule("Function: Type Identifier ExprBrL ( SingleFullDeclaration (Comma SingleFullDeclaration)*)? ExprBrR FunctionBody")]
    private ASTNode Function(ASTType returnType, Tk name, Tk _0, Punctuated<ASTDeclaration, Tk> parameters, Tk _1, ASTStatement? body) =>
        throw new NotImplementedException();

    [Rule("FunctionBody: Semicolon")]
    private ASTStatement? NoFunctionBody(Tk _0) => null;

    [Rule("FunctionBody: StatementScope")]
    private ASTStatement? FullFunctionBody(ASTStatement body) => body;
#endregion

#region Global blocks
    [Rule("Option: KwOption Identifier Semicolon")]
    private ASTGlobalBlock Option(Tk _0, Tk name, Tk _1) =>
        throw new NotImplementedException();

    [Rule("Option: KwOption Identifier Assign (Identifier (Comma Identifier)*) Semicolon")]
    private ASTGlobalBlock Option(Tk _0, Tk name, Tk _2, Punctuated<Tk, Tk> values, Tk _3) =>
        throw new NotImplementedException();

    [Rule("BlockCondition: KwIf ExprBrL Expression ExprBrR")]
    private ASTExpression BlockCondition(Tk _0, Tk _1, ASTExpression condition, Tk _2) => condition;

    [Rule("StorageBlock: StorageBlockKind BlockCondition? Type Identifier Semicolon")]
    private ASTGlobalBlock StorageBlock(TokenKind kind, ASTExpression? condition, ASTType type, Tk name, Tk _0) =>
        throw new NotImplementedException();

    [Rule("StorageBlock: StorageBlockKind BlockCondition? ScopeBrL DeclarationStatement* ScopeBrR")]
    private ASTGlobalBlock StorageBlock(TokenKind kind, ASTExpression? condition, Tk _0, IReadOnlyList<ASTStatement> declarations, Tk _1) =>
        throw new NotImplementedException();

    [Rule("StorageBlockKind: KwAttributes | KwInstances | KwUniform | KwVarying")]
    private TokenKind StorageBlockKind(Tk kind) => kind.Kind;

    [Rule("PipelineDeclaration: Identifier ( Identifier | UnsignedInteger | UnsignedReal )? Semicolon")]
    private ASTNode PipelineDeclarationSingleOrNone(Tk key, Tk? value, Tk _0) =>
        throw new NotImplementedException();

    [Rule("PipelineDeclaration: KwBlend BlendFormula BlendFormula? Semicolon")]
    private ASTNode PipelineDeclarationBlend(Tk _0, BlendFormula color, BlendFormula? alpha, Tk _1) =>
        throw new NotImplementedException();

    [Rule("BlendFormula: Identifier ( Add | Subtract | BitNegate | Lesser | Greater ) Identifier")]
    private BlendFormula BlendFormula(Tk sourceFactor, Tk function, Tk destinationFactor) =>
        throw new NotImplementedException();

    [Rule("PipelineBlock: KwPipeline BlockCondition? PipelineDeclaration")]
    private ASTGlobalBlock PipelineBlock(Tk _0, ASTExpression? condition, ASTNode declaration) =>
        throw new NotImplementedException();

    [Rule("PipelineBlock: KwPipeline BlockCondition? ScopeBrL PipelineDeclaration* ScopeBrR")]
    private ASTGlobalBlock PipelineBlock(Tk _0, ASTExpression? condition, Tk _1, IReadOnlyList<ASTNode> declarations, Tk _2) =>
        throw new NotImplementedException();

    [Rule("StageBlock: ( KwVertex | KwFragment ) ScopeBrL StageItem* ScopeBrR")]
    private ASTGlobalBlock StageBlock(Tk kind, Tk _0, IReadOnlyList<ASTNode> items, Tk _1) =>
        throw new NotImplementedException();

    [Rule("StageItem: Statement | Function")]
    private ASTNode StageItem(ASTNode node) => node;

    [Rule("GlobalBlock: Option | StageBlock | StorageBlock | PipelineBlock")]
    private ASTGlobalBlock GlobalBlock(ASTGlobalBlock node) => node;
#endregion

    // Start symbol
    [Rule("TranslationUnit: GlobalBlock*")]
    private ASTNode TranslationUnit(IReadOnlyList<ASTGlobalBlock> blocks) =>
        throw new NotImplementedException();
}
