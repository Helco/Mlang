using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using Mlang.Model;

namespace Mlang;

internal class ShaderSetFileReader : IDisposable
{
    private readonly Stream stream;
    private readonly bool ownsStream;
    private uint version;
    private long startPositionOfVariants;
    private ShaderSetFile.ShaderHeader[] shaders = Array.Empty<ShaderSetFile.ShaderHeader>();
    private byte[] variantHeaders = Array.Empty<byte>();
    private bool disposedValue;

    public IReadOnlyList<ShaderSetFile.ShaderHeader> Shaders => shaders;
    public Span<ShaderSetFile.VariantHeader> Variants =>
        MemoryMarshal.Cast<byte, ShaderSetFile.VariantHeader>(variantHeaders);

    private ShaderSetFileReader(bool _, Stream stream, bool ownsStream)
    {
        this.stream = stream;
        this.ownsStream = ownsStream;
    }

    public ShaderSetFileReader(Stream stream, bool leaveOpen = false)
        : this(false, stream, !leaveOpen)
    {
        ReadHeaders();
    }

    // to save memory as we cannot use the byte array on .NET Standard 2.0
    public void ClearVariants() => variantHeaders = Array.Empty<byte>();

    private unsafe void ReadHeaders()
    {
        uint variantCount;
        using (var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
        {
            if (reader.ReadUInt32() != ShaderSetFile.Magic)
                throw new InvalidDataException($"Invalid magic value for MLang shader set file");
            version = reader.ReadUInt32();
            if (version != ShaderSetFile.Version)
                throw new NotSupportedException($"Unsupported MLang shader set version: {version}");
            var shaderCount = reader.ReadUInt32();
            variantCount = reader.ReadUInt32();

            shaders = new ShaderSetFile.ShaderHeader[shaderCount];
            var curVariantOffset = 0;
            for (uint i = 0; i < shaderCount; i++)
            {
                shaders[i].Info = ShaderInfo.Read(reader);
                shaders[i].VariantCount = checked((int)reader.ReadUInt32());
                shaders[i].Name = reader.ReadString();
                shaders[i].Source = reader.ReadString();
                if (shaders[i].Source?.Length is 0 or null)
                    shaders[i].Source = null;

                shaders[i].VariantOffset = curVariantOffset;
                curVariantOffset += shaders[i].VariantCount;
            }

            variantHeaders = reader.ReadBytes(sizeof(ShaderSetFile.VariantHeader) * checked((int)variantCount));
        }
        startPositionOfVariants = stream.Position;
    }

    public ShaderVariant ReadVariant(uint shaderHash, ShaderSetFile.VariantHeader variant)
    {
        stream.Position = startPositionOfVariants + variant.Offset;
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        var pipelineState = PipelineState.Read(reader);
        var vertexAttributes = reader.ReadArray(VertexAttributeInfo.Read);
        var bindingSetSizes = reader.ReadArray(static r => r.ReadInt32());
        var bindings = reader.ReadArray(BindingInfo.Read);
        var programOffset = reader.ReadUInt32();
        stream.Position = startPositionOfVariants + programOffset;
        return new ShaderVariant(
            new(shaderHash, variant.OptionBits),
            pipelineState,
            vertexAttributes,
            bindingSetSizes,
            bindings,
            reader.ReadBytes(reader.ReadInt32()),
            reader.ReadBytes(reader.ReadInt32()));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing && ownsStream)
                stream.Dispose();
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
