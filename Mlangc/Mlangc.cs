﻿using System.Text;
using System.Text.Json;
using Mlang;
using Mlang.Language;
using Mlang.Model;
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
option Blend = IsOpaque, IsAlphaBlend, IsAdditiveBlend, IsAdditiveAlphaBlend;

variants exclude if (IsInstanced && IsSkinned);
variants include if (IsInstanced && IsSkinned && HasEnvMap);
variants exclude if (Blend == IsAlphaBlend);

attributes Geometry
{
    float3 inPos;
    float3 inNormal;
    float2 inUV;
    byte4_norm inColor;
}

attributes Skin if (IsSkinned)
{
    float4 inWeights;
    byte4 inIndices;
}

instances mat4 world;
instances if (HasTexShift) mat3x2 inTexShift;
instances Colors
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

pipeline if (Blend == IsAlphaBlend)
{
    blend SrcAlpha + InvSrcAlpha;
}

pipeline if (Blend == IsAdditiveBlend)
{
    blend One + One;
}

pipeline if (Blend == IsAdditiveAlphaBlend)
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
        float3 incident = (world[3] + vec4(inPos, 0) + view[3] + view[2]).xyz;
        uv = vec2(-reflect(incident, inNormal));
    }
    if (HasTexShift)
        uv = inTexShift * vec3(uv, 1.4);
	varUV = uv;

	varColor = inColor;
}

fragment
{
    vec4 color = texture(sampler2D(mainTexture, mainSampler), varUV)
		* weighColor(varColor, inVertexColorFactor)
		* weighColor(inTint, inTintFactor);
	if (color.a < inAlphaReference)
		discard;
	outColor = color;
}
";

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Environment.CurrentDirectory = Path.GetDirectoryName(Environment.ProcessPath ?? ".") ?? ".";
        File.WriteAllText("model.mlang", Shader);
        File.WriteAllText("model2.mlang", Shader + "\n//comment to change source hash");

        var task = new CompileMlangShaderSet()
        {
            ShaderFiles = new[] { "model.mlang", "model2.mlang" },
            /*ShaderFiles = new[]
            {
                
                
                @"C:\dev\zzio\zzre\shaders\model.mlang",

                @"C:\dev\zzio\zzre\shaders\debug.mlang",
                @"C:\dev\zzio\zzre\shaders\ui.mlang",
                @"C:\dev\zzio\zzre\shaders\effect.mlang",
            },*/
            EmbedShaderSource = true,
            OutputGeneratedSourceOnError = true,
            OutputPath = "model.shadercache",
            RunInParallel = true
        };
        if (!task.Execute())
            return;

        var shaderSet = new FileShaderSet("model.shadercache");
        Console.WriteLine($"Loaded shader set with {shaderSet.TotalVariantCount} variants");
        shaderSet.LoadAll();
        var modelShaderInfo = shaderSet.GetShaderInfo("model");
        var modelVariant = shaderSet.GetVariant(new ShaderVariantKey(modelShaderInfo.SourceHash, 0u));
        foreach (var attr in modelVariant.VertexAttributes)
            Console.WriteLine($"{attr.Type.MlangName} {attr.Name}");
    }
}