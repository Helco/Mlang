using System;
using Mlang.Model;
using Yoakke.SynKit.Text;
using Tk = Yoakke.SynKit.Lexer.IToken<Mlang.Language.TokenKind>;

namespace Mlang.Language;

partial class Parser
{
    private PartialPipelineState ParsePipelineVectorDeclaration(Tk key, float x, float y, float z, float w)
    {
        if (key.Text.ToLowerInvariant() != "blendfactor")
            diagnostics.Add(Mlang.Diagnostics.DiagUnknownPipelineStateForVector(SourceFile, key));
        return new()
        {
            BlendFactor = new(x, y, z, w)
        };
    }

    private PartialPipelineState ParsePipelineTwoValueDeclaration(Tk key, Tk value1, Tk value2)
    {
        if (key.Text.ToLowerInvariant() != "output")
            diagnostics.Add(Mlang.Diagnostics.DiagUnknownPipelineStateForTwoArguments(SourceFile, key));
        return new()
        {
            ColorOutputs = [new(value2.Text, ParsePixelFormat(value1))]
        };
    }

    private PartialPipelineState ParsePipelineValueDeclaration(Tk key, Tk value)
    {
        var state = new PartialPipelineState();
        switch(key.Text.ToLowerInvariant())
        {
            case "alphatocoverage": state.AlphaToCoverage = ParseBooleanValue("AlphaToCoverage", value); break;
            case "depthtest": state.DepthTest = ParseBooleanValue("DepthTest", value); break;
            case "depthwrite": state.DepthWrite = ParseBooleanValue("DepthWrite", value); break;
            case "stenciltest": state.StencilTest = ParseBooleanValue("StencilTest", value); break;
            case "depthclip": state.DepthClip = ParseBooleanValue("DepthClip", value); break;
            case "scissortest": state.ScissorTest = ParseBooleanValue("ScissorTest", value); break;

            case "stencilreadmask": state.StencilReadMask = (byte)ParseInteger("StencilReadMask", value, byte.MaxValue); break;
            case "stencilwritemask": state.StencilWriteMask = (byte)ParseInteger("StencilWriteMask", value, byte.MaxValue); break;
            case "stencilreference": state.StencilReference = (uint)ParseInteger("StencilReference", value, uint.MaxValue); break;

            case "stencilcomparison": state.Stencil.Comparison = ParseComparisonKind(value); break;
            case "stencilpass": state.Stencil.Pass = ParseStencilOperation(value); break;
            case "stencilfail": state.Stencil.Fail = ParseStencilOperation(value); break;
            case "stencildepthfail": state.Stencil.DepthFail = ParseStencilOperation(value); break;
            case "stencilfrontcomparison": state.StencilFront.Comparison = ParseComparisonKind(value); break;
            case "stencilfrontpass": state.StencilFront.Pass = ParseStencilOperation(value); break;
            case "stencilfrontfail": state.StencilFront.Fail = ParseStencilOperation(value); break;
            case "stencilfrontdepthfail": state.StencilFront.DepthFail = ParseStencilOperation(value); break;
            case "stencilbackcomparison": state.StencilBack.Comparison = ParseComparisonKind(value); break;
            case "stencilbackpass": state.StencilBack.Pass = ParseStencilOperation(value); break;
            case "stencilbackfail": state.StencilBack.Fail = ParseStencilOperation(value); break;
            case "stencilbackdepthfail": state.StencilBack.DepthFail = ParseStencilOperation(value); break;

            case "cullmode":
            case "cull": state.CullMode = ParseFaceCullMode(value); break;
            case "fillmode":
            case "fill": state.FillMode = ParsePolygonFillMode(value); break;
            case "frontface": state.FrontFace = ParseFrontFace(value); break;
            case "primitivetopology":
            case "topology": state.PrimitiveTopology = ParsePrimitiveTopology(value); break;
            case "output": state.DepthOutput = ParsePixelFormat(value); break;

            default:
                diagnostics.Add(Mlang.Diagnostics.DiagUnknownPipelineStateForArgument(SourceFile, key));
                break;
        }
        return state;
    }

    private PartialPipelineState ParsePipelineFactDeclaration(Tk key)
    {
        var state = new PartialPipelineState();
        switch (key.Text.ToLowerInvariant())
        {
            case "alphatocoverage": state.AlphaToCoverage = true; break;
            case "depthtest": state.DepthTest = true; break;
            case "depthwrite": state.DepthWrite = true; break;
            case "stenciltest": state.StencilTest = true; break;
            case "depthclip": state.DepthClip = true; break;
            case "scissortest": state.ScissorTest = true; break;

            case var _ when Enum.TryParse<FaceFillMode>(key.Text, ignoreCase: true, out var fillMode):
                state.FillMode = fillMode;
                break;
            case var _ when Enum.TryParse<PrimitiveTopology>(key.Text, ignoreCase: true, out var topology):
                state.PrimitiveTopology = topology;
                break;

            default:
                diagnostics.Add(Mlang.Diagnostics.DiagUnknownPipelineFactState(SourceFile, key));
                break;
        }
        return state;
    }

    private bool ParseBooleanValue(string context, Tk value)
    {
        if (value.Kind is TokenKind.Identifier or TokenKind.UnsignedInteger)
        {
            var text = value.Text.ToLowerInvariant();
            if (text is "true" or "yes" or "on" or "enable" or "enabled" or "1")
                return true;
            if (text is "false" or "no" or "off" or "disable" or "disabled" or "0")
                return false;
            diagnostics.Add(Mlang.Diagnostics.DiagExpectedBooleanValue(SourceFile, value.Range, context, value.Text));
        }
        diagnostics.Add(Mlang.Diagnostics.DiagExpectedBooleanValue(SourceFile, value.Range, context, value.Kind.ToString()));
        return false;
    }

    private ulong ParseInteger(string context, Tk token, ulong maxValue)
    {
        if (token.Kind != TokenKind.UnsignedInteger)
        {
            diagnostics.Add(Mlang.Diagnostics.DiagExpectedIntegerValue(SourceFile, token.Range, context, token.Kind.ToString()));
            return 0;
        }
        if (!ulong.TryParse(context, out var value) || value > maxValue)
            diagnostics.Add(Mlang.Diagnostics.DiagIntegerValueTooLarge(SourceFile, token.Range, context, maxValue));
        return value;
    }

    private BlendFunction ParseBlendFunction(Tk token)
    {
        switch (token.Kind)
        {
            case TokenKind.Identifier:
                if (!Enum.TryParse<BlendFunction>(token.Text, ignoreCase: true, out var result))
                    diagnostics.Add(Mlang.Diagnostics.DiagUnknownBlendFunction(SourceFile, token.Range, token.Text));
                return result;
            case TokenKind.Add: return BlendFunction.Add;
            case TokenKind.Subtract: return BlendFunction.Subtract;
            case TokenKind.BitNegate: return BlendFunction.ReverseSubtract;
            case TokenKind.Greater: return BlendFunction.Maximum;
            case TokenKind.Lesser: return BlendFunction.Minimum;
            default:
                diagnostics.Add(Mlang.Diagnostics.DiagUnknownBlendFunction(SourceFile, token.Range, token.Kind.ToString()));
                return default;
        }
    }

    private ComparisonKind ParseComparisonKind(Tk token)
    {
        switch (token.Kind)
        {
            case TokenKind.Identifier:
                if (!Enum.TryParse<ComparisonKind>(token.Text, ignoreCase: true, out var result))
                    diagnostics.Add(Mlang.Diagnostics.DiagUnknownComparisonKind(SourceFile, token.Range, token.Text));
                return result;
            case TokenKind.Assign: return ComparisonKind.Always;
            case TokenKind.Ampersand: return ComparisonKind.Never;
            case TokenKind.Equals: return ComparisonKind.Equal;
            case TokenKind.NotEquals: return ComparisonKind.NotEqual;
            case TokenKind.Lesser: return ComparisonKind.Less;
            case TokenKind.LessOrEquals: return ComparisonKind.LessEqual;
            case TokenKind.Greater: return ComparisonKind.Greater;
            case TokenKind.GreaterOrEquals: return ComparisonKind.GreaterEqual;
            default:
                diagnostics.Add(Mlang.Diagnostics.DiagUnknownComparisonKind(SourceFile, token.Range, token.Kind.ToString()));
                return default;
        }
    }

    private StencilOperation ParseStencilOperation(Tk token) =>
        ParseEnum<StencilOperation>(token, Mlang.Diagnostics.DiagUnknownStencilOperation);

    private FaceCullMode ParseFaceCullMode(Tk token) =>
        ParseEnum<FaceCullMode>(token, Mlang.Diagnostics.DiagUnknownFaceCullMode);

    private FrontFace ParseFrontFace(Tk token) =>
        ParseEnum<FrontFace>(token, Mlang.Diagnostics.DiagUnknownFrontFace);

    private FaceFillMode ParsePolygonFillMode(Tk token) =>
        ParseEnum<FaceFillMode>(token, Mlang.Diagnostics.DiagUnknownPolygonFillMode);

    private PrimitiveTopology ParsePrimitiveTopology(Tk token) =>
        ParseEnum<PrimitiveTopology>(token, Mlang.Diagnostics.DiagUnknownPrimitiveTopology);

    private PixelFormat ParsePixelFormat(Tk token) =>
        ParseEnum<PixelFormat>(token, Mlang.Diagnostics.DiagUnknownPixelFormat);

    private TEnum ParseEnum<TEnum>(Tk token, Func<ISourceFile, Tk, Diagnostic> createDiagnostic) where TEnum : struct
    {
        TEnum result = default;
        if (token.Kind != TokenKind.Identifier || !Enum.TryParse(token.Text, ignoreCase: true, out result))
            diagnostics.Add(createDiagnostic(SourceFile, token));
        return result;
    }
}
