using System;
using System.Collections.Generic;
using System.Linq;
using Mlang.Model;
using Yoakke.SynKit.Parser;
using Yoakke.SynKit.Parser.Attributes;
using Yoakke.SynKit.Text;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang.Language;

[Parser(typeof(TokenKind))]
internal partial class Parser
{
    private List<Diagnostic> diagnostics = new();
    private int nextOptionIndex = 0;
    private int nextOptionBitOffset = 0;

    public required ISourceFile SourceFile { get; init; }
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
    private ASTExpression PrimaryExpressionInt(Tk literal) => new ASTIntegerLiteral()
    {
        Range = literal.Range,
        Value = long.Parse(literal.Text)
    };

    [Rule("PrimaryExpression: UnsignedReal")]
    private ASTExpression PrimaryExpressionReal(Tk literal) => new ASTRealLiteral()
    {
        Range = literal.Range,
        Value = double.Parse(literal.Text)
    };
    
    [Rule("PrimaryExpression: Identifier")]
    private ASTExpression PrimaryExpressionVar(Tk varIdentifier) => new ASTVariable()
    {
        Range = varIdentifier.Range,
        Name = varIdentifier.Text
    };

    [Rule("PostfixExpression: PostfixExpression ExprBrL (Expression (Comma AssignmentExpression)*)? ExprBrR")]
    private ASTExpression PostfixExpressionCall(ASTExpression function, Tk _0, Punctuated<ASTExpression, Tk> parameters, Tk _1) => new ASTFunctionCall()
    {
        Range = new(function.Range, _1.Range),
        Function = function,
        Parameters = parameters.Values.ToArray()
    };

    [Rule("PostfixExpression: PostfixExpression ArrayBrL Expression ArrayBrR")]
    private ASTExpression PostfixExpressionArray(ASTExpression array, Tk _0, ASTExpression index, Tk _1) => new ASTArrayAccess()
    {
        Range = new(_0.Range, _1.Range),
        Array = array,
        Index = index  
    };

    [Rule("PostfixExpression: PostfixExpression ( Increment | Decrement )")]
    private ASTExpression PostfixExpressionUnary(ASTExpression operand, Tk op) => new ASTPostUnaryExpression()
    {
        Range = op.Range,
        Operand = operand,
        Operator = op.Kind
    };

    [Rule("PostfixExpression: PostfixExpression Dot Identifier")]
    private ASTExpression PostfixExpressionMember(ASTExpression parent, Tk _0, Tk member) => new ASTMemberAccess()
    {
        Range = member.Range,
        Parent = parent,
        Member = member.Text
    };

    [Rule("UnaryExpression: ( Increment | Decrement | Add | Subtract | Ampersand | BitNegate ) UnaryExpression")]
    private ASTExpression UnaryExpressionPrefix(Tk op, ASTExpression value) => new ASTUnaryExpression()
    {
        Range = op.Range,
        Operand = value,
        Operator = op.Kind
    };

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
    private ASTExpression BinaryExpression(ASTExpression left, Tk op, ASTExpression right) => new ASTBinaryExpression()
    {
        Range = op.Range,
        Left = left,
        Operator = op.Kind,
        Right = right
    };

    [Rule("ConditionalExpression: LogicalOrExpression Question Expression Colon AssignmentExpression")]
    private ASTExpression ConditionalExpression(ASTExpression condition, Tk _0, ASTExpression thenValue, Tk _1, ASTExpression elseValue) => new ASTConditional()
    {
        Range = _0.Range,
        Condition = condition,
        Then = thenValue,
        Else = elseValue
    };
#endregion

#region Statements
    [Rule("Statement: EmptyStatement | DeclarationStatement | ExpressionStatement | SelectionStatement | SwitchStatement | ForLoop | WhileLoop | DoWhileLoop | ReturnStatement | FlowStatement | StatementScope")]
    [Rule("SwitchBodyStatement: Statement | CaseLabel | DefaultLabel")]
    [Rule("ForInitStatement: DeclarationStatement | ExpressionStatement")]
    private ASTStatement Statement(ASTStatement stmt) => stmt;

    [Rule("EmptyStatement: Semicolon")]
    private ASTStatement EmptyStatement(Tk _0) => new ASTEmptyStatement()
    {
        Range = _0.Range
    };

    [Rule("ExpressionStatement: Expression Semicolon")]
    private ASTStatement ExpressionStatement(ASTExpression expr, Tk _0) => new ASTExpressionStatement()
    {
        Range = expr.Range,
        Expression = expr
    };

    [Rule("SelectionStatement: KwIf ExprBrL Expression ExprBrR Statement ElseStatement?")]
    private ASTStatement SelectionStatement(Tk _0, Tk _1, ASTExpression condition, Tk _2, ASTStatement thenBody, ASTStatement? elseBody) => new ASTSelection()
    {
        Range = _0.Range,
        Condition = condition,
        Then = thenBody,
        Else = elseBody
    };

    [Rule("ElseStatement: KwElse Statement")]
    private ASTStatement ElseStatement(Tk _0, ASTStatement body) => body;

    [Rule("SwitchStatement: KwSwitch ExprBrL Expression ExprBrR ScopeBrL SwitchBodyStatement* ScopeBrR")]
    private ASTStatement SwitchStatement(Tk _0, Tk _1, ASTExpression value, Tk _2, Tk _3, IReadOnlyList<ASTStatement> body, Tk _4) => new ASTSwitchStatement()
    {
        Range = _0.Range,
        Value = value,
        Body = body.ToArray()
    };

    [Rule("CaseLabel: KwCase Expression Colon")]
    private ASTStatement CaseLabel(Tk _0, ASTExpression value, Tk _1) => new ASTCaseLabel()
    {
        Range = _0.Range,
        Value = value
    };

    [Rule("DefaultLabel: KwDefault Colon")]
    private ASTStatement DefaultLabel(Tk _0, Tk _1) => new ASTDefaultLabel()
    {
        Range = _0.Range
    };

    [Rule("ForLoop: KwFor ExprBrL ForInitStatement Expression Semicolon Expression? ExprBrR Statement")]
    private ASTStatement ForLoop(Tk _0, Tk _1, ASTStatement init, ASTExpression condition, Tk _2, ASTExpression? update, Tk _3, ASTStatement body) => new ASTForLoop()
    {
        Range = _0.Range,
        Init = init,
        Condition = condition,
        Update = update,
        Body = body
    };

    [Rule("WhileLoop: KwWhile ExprBrL Expression ExprBrR Statement")]
    private ASTStatement WhileLoop(Tk _0, Tk _1, ASTExpression condition, Tk _2, ASTStatement body) => new ASTWhileLoop()
    {
        Range = _0.Range,
        Condition = condition,
        Body = body
    };

    [Rule("DoWhileLoop: KwDo Statement KwWhile ExprBrL Expression ExprBrR Semicolon")]
    private ASTStatement DoWhileLoop(Tk _0, ASTStatement body, Tk _1, Tk _2, ASTExpression condition, Tk _3, Tk _4) => new ASTDoWhileLoop()
    {
        Range = _0.Range,
        Condition = condition,
        Body = body
    };

    [Rule("ReturnStatement: KwReturn Expression? Semicolon")]
    private ASTStatement ReturnStatement(Tk _0, ASTExpression? value, Tk _1) => new ASTReturn()
    {
        Range = _0.Range,
        Value = value
    };

    [Rule("FlowStatement: (KwBreak | KwContinue | KwDiscard) Semicolon")]
    private ASTStatement FlowStatement(Tk instruction, Tk _0) => new ASTFlowStatement()
    {
        Range = instruction.Range,
        Instruction = instruction.Kind
    };

    [Rule("DeclarationStatement: Type (DeclarationWithInitializer (Comma DeclarationWithInitializer)*) Semicolon")]
    private ASTDeclarationStatement DeclarationStatement(ASTType type, Punctuated<(Tk name, ASTExpression? value), Tk> declarations, Tk _0) => new ASTDeclarationStatement()
    {
        Range = type.Range,
        Declarations = declarations.Values.Select(t => new ASTDeclaration()
        {
            Range = t.name.Range,
            Type = type,
            Name = t.name.Text,
            Initializer = t.value
        }).ToArray()
    };

    [Rule("StatementScope: ScopeBrL Statement* ScopeBrR")]
    private ASTStatement StatementScope(Tk _0, IReadOnlyList<ASTStatement> body, Tk _1) => new ASTStatementScope()
    {
        Range = _0.Range,
        Body = body.ToArray()
    };
#endregion

#region Types and declarations
    [Rule("Type: Identifier")]
    private ASTType NamedType(Tk name) => name.Text switch
    {
        _ when NumericType.TryParse(name.Text, out var numericType) => new ASTNumericType()
        {
            Range = name.Range,
            Type = numericType
        },
        _ when ImageType.TryParse(name.Text, out var imageType) => new ASTImageType()
        {
            Range = name.Range,
            Type = imageType
        },
        _ when ImageType.TryParseSamplerType(name.Text, out var samplerType) => new ASTSamplerType()
        {
            Range = name.Range,
            Type = samplerType
        },
        _ => new ASTCustomType()
        {
            Range = name.Range,
            Type = name.Text
        }
    };

    [Rule("Type: KwBuffer Type")]
    private ASTType QualifiedType(Tk qualifier, ASTType type) => new ASTBufferType()
    {
        Range = qualifier.Range,
        Inner = type
    };

    [Rule("Type: Type ArrayBrL Expression? ArrayBrR")]
    private ASTType ArrayType(ASTType element, Tk _0, ASTExpression? size, Tk _1) => new ASTArrayType()
    {
        Range = new(_0.Range, _1.Range),
        Element = element,
        Size = size
    };

    [Rule("DeclarationWithInitializer: Identifier DeclarationInitializer?")]
    private (Tk name, ASTExpression? value) DeclarationWithInitializer(Tk name, ASTExpression? value) =>
        (name, value);

    [Rule("DeclarationInitializer: Assign LogicalOrExpression")]
    private ASTExpression DeclarationInitializer(Tk _0, ASTExpression value) => value;

    [Rule("SingleFullDeclaration: Type Identifier DeclarationInitializer?")]
    private ASTDeclaration SingleFullDeclaration(ASTType type, Tk name, ASTExpression? value) => new ASTDeclaration()
    {
        Range = name.Range,
        Type = type,
        Name = name.Text,
        Initializer = value
    };
#endregion

#region Functions
    [Rule("Function: Type Identifier ExprBrL ( SingleFullDeclaration (Comma SingleFullDeclaration)*)? ExprBrR FunctionBody")]
    private ASTNode Function(ASTType returnType, Tk name, Tk _0, Punctuated<ASTDeclaration, Tk> parameters, Tk _1, ASTStatement? body) => new ASTFunction()
    {
        Range = name.Range,
        ReturnType = returnType is ASTCustomType { Type: "void "} ? null : returnType,
        Name = name.Text,
        Parameters = parameters.Values.ToArray()
    };

    [Rule("FunctionBody: Semicolon")]
    private ASTStatement? NoFunctionBody(Tk _0) => null;

    [Rule("FunctionBody: StatementScope")]
    private ASTStatement? FullFunctionBody(ASTStatement body) => body;
#endregion

#region Global blocks
    [Rule("Option: KwOption Identifier Semicolon")]
    private ASTGlobalBlock Option(Tk _0, Tk name, Tk _1) => new ASTOption()
    {
        Range = name.Range,
        Name = name.Text,
        Index = nextOptionIndex++,
        BitOffset = nextOptionBitOffset++,
        NamedValues = null
    };

    [Rule("Option: KwOption Identifier Assign (Identifier (Comma Identifier)*) Semicolon")]
    private ASTGlobalBlock Option(Tk _0, Tk name, Tk _2, Punctuated<Tk, Tk> values, Tk _3)
    {
        var option = new ASTOption()
        {
            Range = name.Range,
            Name = name.Text,
            Index = nextOptionIndex++,
            BitOffset = nextOptionBitOffset,
            NamedValues = values.Select(v => v.Value.Text).ToArray()
        };
        int bitCount = option.BitCount;
        if (bitCount < 1)
            diagnostics.Add(Mlang.Diagnostics.DiagTooFewOptions(SourceFile, option.Range));
        nextOptionBitOffset += bitCount;
        return option;
    }

    [Rule("BlockCondition: KwIf ExprBrL Expression ExprBrR")]
    private ASTExpression BlockCondition(Tk _0, Tk _1, ASTExpression condition, Tk _2) => condition;

    [Rule("StorageBlock: StorageBlockKind BlockCondition? Type Identifier Semicolon")]
    private ASTGlobalBlock StorageBlock(Tk kind, ASTExpression? condition, ASTType type, Tk name, Tk _0) => new ASTStorageBlock()
    {
        Range = kind.Range,
        StorageKind = kind.Kind,
        Condition = condition,
        Declarations = [new ASTDeclaration()
        {
            Range = name.Range,
            Type = type,
            Name = name.Text
        }]
    };

    [Rule("StorageBlock: StorageBlockKind BlockCondition? ScopeBrL DeclarationStatement* ScopeBrR")]
    private ASTGlobalBlock StorageBlock(Tk kind, ASTExpression? condition, Tk _0, IReadOnlyList<ASTDeclarationStatement> declarations, Tk _1) => new ASTStorageBlock()
    {
        Range = kind.Range,
        StorageKind = kind.Kind,
        Condition = condition,
        Declarations = declarations.SelectMany(d => d.Declarations).ToArray()
    };

    [Rule("StorageBlockKind: KwAttributes | KwInstances | KwUniform | KwVarying")]
    private Tk StorageBlockKind(Tk kind) => kind;

    [Rule("PipelineDeclaration: Identifier ( Identifier | UnsignedInteger | UnsignedReal | Assign | Ampersand | Equals | NotEquals | Lesser | Greater | LessOrEquals | GreaterOrEquals )? Semicolon")]
    private PartialPipelineState PipelineDeclarationSingleOrNone(Tk key, Tk? value, Tk _0) => value == null
        ? ParsePipelineFactDeclaration(key)
        : ParsePipelineValueDeclaration(key, value);

    [Rule("PipelineDeclaration: Identifier AnyNumber AnyNumber AnyNumber AnyNumber Semicolon")]
    private PartialPipelineState PipelineDeclarationVector(Tk key, float x, float y, float z, float w, Tk _0) =>
        ParsePipelineVectorDeclaration(key, x, y, z, w);

    [Rule("AnyNumber: Subtract? (UnsignedInteger | UnsignedReal)")]
    private float AnyNumber(Tk? minus, Tk unsigned) =>
        (minus == null ? 1.0f : -1.0f) * float.Parse(unsigned.Text);

    [Rule("PipelineDeclaration: KwBlend BlendFormula BlendFormula? Semicolon")]
    private PartialPipelineState PipelineDeclarationBlend(Tk _0, BlendFormula color, BlendFormula? alpha, Tk _1) => new PartialPipelineState()
    {
        BlendAttachments = [new(color, alpha)]
    };

    [Rule("BlendFormula: Identifier ( Identifier | Add | Subtract | BitNegate | Lesser | Greater ) Identifier")]
    private BlendFormula BlendFormula(Tk sourceFactor, Tk function, Tk destinationFactor) => new(
        ParseEnum<BlendFactor>(sourceFactor, Mlang.Diagnostics.DiagUnknownBlendFactor),
        ParseEnum<BlendFactor>(destinationFactor, Mlang.Diagnostics.DiagUnknownBlendFactor),
        ParseBlendFunction(function)
    );

    [Rule("PipelineBlock: KwPipeline BlockCondition? PipelineDeclaration")]
    private ASTGlobalBlock PipelineBlock(Tk _0, ASTExpression? condition, PartialPipelineState declaration) => new ASTPipelineBlock()
    {
        Range = _0.Range,
        Condition = condition,
        State = declaration
    };

    [Rule("PipelineBlock: KwPipeline BlockCondition? ScopeBrL PipelineDeclaration* ScopeBrR")]
    private ASTGlobalBlock PipelineBlock(Tk _0, ASTExpression? condition, Tk _1, IReadOnlyList<PartialPipelineState> declarations, Tk _2)
    {
        var state = new PartialPipelineState();
        foreach (var decl in declarations)
            state.With(decl);
        return new ASTPipelineBlock()
        {
            Range = _0.Range,
            Condition = condition,
            State = state
        };
    }

    [Rule("StageBlock: ( KwVertex | KwFragment ) BlockCondition? ScopeBrL StageItem* ScopeBrR")]
    private ASTGlobalBlock StageBlock(Tk kind, ASTExpression? condition, Tk _0, IReadOnlyList<ASTNode> items, Tk _1) => new ASTStageBlock()
    {
        Range = kind.Range,
        Stage = kind.Kind,
        Condition = condition,
        Functions = items.OfType<ASTFunction>().ToArray(),
        Statements = items.OfType<ASTStatement>().ToArray()
    };

    [Rule("StageItem: Statement | Function")]
    private ASTNode StageItem(ASTNode node) => node;

    [Rule("GlobalBlock: Option | StageBlock | StorageBlock | PipelineBlock")]
    private ASTGlobalBlock GlobalBlock(ASTGlobalBlock node) => node;
#endregion

    // Start symbol
    [Rule("TranslationUnit: GlobalBlock*")]
    private ASTNode TranslationUnit(IReadOnlyList<ASTGlobalBlock> blocks) => new ASTTranslationUnit()
    {
        Range = default,
        Blocks = blocks.ToArray()
    };
}
