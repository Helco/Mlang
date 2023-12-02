﻿using Mlang.Language;
using Yoakke.SynKit.Reporting.Present;
using Yoakke.SynKit.Text;

namespace Mlangc;

internal class Mlangc
{
    const string Shader = @"
option IsInstanced;
option IsSkinned;
option HasTexShift;
option HasEnvMap;
option Blend = isOpaque, isAlphaBlend, isAdditiveBlend, isAdditiveAlphaBlend;

attributes
{
    float3 inPos;
    float3 inNormal;
    float2 inUV;
    byte4_norm inColor;
}

attributes if (IsSkinned)
{
    float4 inWeights;
    byte4 inIndices;
}

instances mat4 world;
instances if (HasTexShift) mat3x2 inTexShift;
instances
{
    byte4_norm inTint;
    float inVertexColorFactor;
    float inTintFactor;
    float inAlphaReference;
}

varying
{
    float2 varUV;
    float4 varColor;
}

uniform texture2D mainTexture;
uniform sampler mainSampler;
uniform mat4 projection;
uniform mat4 view;
uniform if (IsSkinned) buffer mat4[] pose;

pipeline
{
    blend One + Zero;
    output r8_g8_b8_a8_unorm outColor;
    output d24_unorm_s8_uint;
}

pipeline if (IsAlphaBlend)
{
    blend SrcAlpha + InvSrcAlpha;
}

pipeline if (IsAdditiveBlend)
{
    blend One + One;
}

pipeline if (IsAdditiveAlphaBlend)
{
    blend SrcAlpha + One;
}

vec4 weighColor(vec4 color, float factor)
{
	return color * factor + vec4(1,1,1,1) * (1 - factor);
}

vertex
{
    vec4 pos = vec4(inPos, 1);

    if (IsSkinned)
    {
        pos =
            (pose[inIndices.x] * pos) * inWeights.x +
            (pose[inIndices.y] * pos) * inWeights.y +
            (pose[inIndices.z] * pos) * inWeights.z +
            (pose[inIndices.w] * pos) * inWeights.w;
        pos.w = 1; // TODO: Check if this is necessary
    }

	pos = world * pos;
	pos = view * pos;
	pos = projection * pos;
	gl_Position = pos;
    
    vec2 uv = inUV;
    if (HasEnvMap)
    {
        float3 incident = world[3] + inPos + view[3] + view[2];
        uv = -reflect(incident, inNormal);
    }
    if (HasTexShift)
        uv = texShift * uv;
	varUV = uv;

	varColor = inColor;
}

fragment
{
    vec4 color = texture(sampler2D(mainTexture, mainSampler), varUV)
		* weighColor(varColor, inVectorColorFactor)
		* weighColor(inTint, inTintFactor);
	if (color.a < inAlphaReference)
		discard;
	outColor = color;
}
";

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var compiler = new Compiler("model.mlang", Shader);
        Console.Write("Parsing...");
        Console.WriteLine(compiler.ParseShader() ? "success" : "failure");

        var presenter = new TextDiagnosticsPresenter(Console.Error);
        foreach (var diagnostic in compiler.Diagnostics)
            presenter.Present(diagnostic.ConvertToSynKit());

        if (!compiler.HasError)
        {
            using var writer = new Mlang.CodeWriter(Console.Out, disposeWriter: false);
            compiler.unit!.Write(writer);
        }
    }
}