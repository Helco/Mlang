using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using Mlang.Model;

namespace Mlang;

public class ShaderSetFileWriter : IDisposable
{
    private readonly Stream stream;
    private readonly bool ownsStream;
    private readonly BinaryWriter writer;
    private readonly List<ShaderSetFile.ShaderHeader> shaders = new();
    private readonly List<int> nextShaderVariants = new();
    private readonly Dictionary<ShaderVariantKey, uint> invariantProgramOffsets = new();
    private byte[]? variantBytes = null;
    private long startPositionOfHeaders, startPositionOfVariants;
    private int nextVariant;
    private bool disposedValue;
    private bool finished;

    private Span<ShaderSetFile.VariantHeader> Variants =>
        MemoryMarshal.Cast<byte, ShaderSetFile.VariantHeader>(variantBytes);

    public ShaderSetFileWriter(Stream stream, bool leaveOpen = false)
    {
        this.stream = stream;
        ownsStream = !leaveOpen;
        writer = new(stream, Encoding.UTF8, leaveOpen: true);
    }

    public void AddShader(ShaderInfo info, string name, string? source, int variantCount)
    {
        ThrowIfFinished();
        if (variantBytes != null)
            throw new InvalidOperationException("First variant was written, cannot add shaders anymore");
        if (variantCount < 0)
            throw new ArgumentException("Variant count cannot be negative");
        shaders.Add(new()
        {
            Info = info,
            Name = name,
            Source = source,
            VariantOffset = nextVariant,
            VariantCount = variantCount
        });
        nextShaderVariants.Add(0);
        nextVariant += variantCount;
    }

    private unsafe void PrepareForVariantWriting()
    {
        if (variantBytes != null)
            return;
        int totalCount = shaders.Sum(s => s.VariantCount);
        variantBytes = new byte[sizeof(ShaderSetFile.VariantHeader) * totalCount];
        for (int i = 0; i < totalCount; i++) // No Array.Fill in .NET Standard 2.0
            Variants[i] = new() { OptionBits = uint.MaxValue, Offset = uint.MaxValue };

        writer.Write(ShaderSetFile.Magic);
        writer.Write(ShaderSetFile.Version);
        writer.Write(shaders.Count);
        writer.Write(totalCount);
        foreach (var shader in shaders)
        {
            shader.Info.Write(writer);
            writer.Write(shader.VariantCount);
            writer.Write(shader.Name);
            writer.Write(shader.Source ?? "");
        }
        startPositionOfHeaders = stream.Position;
        startPositionOfVariants = stream.Position + variantBytes.LongLength;
        stream.Position += variantBytes.LongLength;
    }

    public void WriteVariant(ShaderVariant variant)
    {
        ThrowIfFinished();
        PrepareForVariantWriting();
        int shaderI = shaders.FindIndex(s => s.Info.SourceHash == variant.VariantKey.ShaderHash);
        if (shaderI < 0)
            throw new ArgumentException("Shader was not added to writer");
        var shaderInfo = shaders[shaderI];
        int variantI = nextShaderVariants[shaderI]++;
        if (variantI >= shaderInfo.VariantCount)
            throw new InvalidOperationException("All allocated variants for this shader were already written");
        variantI += shaderInfo.VariantOffset;

        Variants[variantI] = new()
        {
            OptionBits = variant.VariantKey.OptionBits,
            Offset = checked((uint)(stream.Position - startPositionOfVariants))
        };
        variant.PipelineState.Write(writer);
        writer.Write(variant.VertexAttributes, VertexAttributeInfo.Write);
        writer.Write(variant.BindingSetSizes, static (w, i) => w.Write(i));
        writer.Write(variant.Bindings, BindingInfo.Write);

        var invariantKey = shaderInfo.Info.GetProgramInvariantKey(variant.VariantKey);
        if (invariantProgramOffsets.TryGetValue(invariantKey, out var previousOffset))
            writer.Write(previousOffset);
        else
        {
            var newOffset = checked((uint)(stream.Position - startPositionOfVariants + sizeof(uint)));
            invariantProgramOffsets.Add(invariantKey, newOffset);
            writer.Write(newOffset);
            writer.Write(variant.VertexShader.Length);
            writer.Write(variant.VertexShader.ToArray());
            writer.Write(variant.FragmentShader.Length);
            writer.Write(variant.FragmentShader.ToArray());
        }
    }

    public void Finish()
    {
        if (finished)
            return;
        writer.Dispose();

        if (variantBytes != null)
        {
            long endPosition = stream.Position;
            stream.Position = startPositionOfHeaders;
            stream.Write(variantBytes, 0, variantBytes!.Length);
            stream.Position = endPosition;
        }
        if (ownsStream)
            stream.Dispose();
        finished = true;
    }

    private void ThrowIfFinished()
    {
        if (finished)
            throw new InvalidOperationException("Writer was already finished, cannot modify anymore");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            Finish();
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
