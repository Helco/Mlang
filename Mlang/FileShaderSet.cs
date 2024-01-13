using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Mlang.Model;

namespace Mlang;

public class FileShaderSet : IShaderSet
{
    private readonly ShaderSetFileReader reader;

    private readonly Dictionary<uint, int> shadersByHash = new();
    private readonly Dictionary<ShaderVariantKey, ShaderVariant> loadedVariants = new();
#if NET5_0_OR_GREATER
    private Span<ShaderSetFile.VariantHeader> allVariants => reader.Variants;
#else
    private readonly ShaderSetFile.VariantHeader[] allVariants;
#endif

    public IShaderSet? FallbackShaderSet { get; set; }
    public int TotalVariantCount => allVariants.Length;
    public int LoadedVariantCount => loadedVariants.Count;

    public FileShaderSet(string path) : this(new FileStream(path, FileMode.Open, FileAccess.Read)) { }

    public FileShaderSet(Stream stream, bool leaveOpen = false)
    {
        reader = new ShaderSetFileReader(stream, leaveOpen);
#if NETSTANDARD2_0
        // To sort them ´without MemoryExtensions.Sort we have to copy into a typed array
        allVariants = reader.Variants.ToArray();
        reader.ClearVariants(); // we do not need the old memory though
#endif
        foreach (var (index, shader) in reader.Shaders.Indexed())
        {
            shadersByHash[shader.Info.SourceHash] = index;
            // sort by option bits now and we can use binary search without much memory overhead
#if NET5_0_OR_GREATER
            MemoryExtensions.Sort(VariantsOf(shader), OptionBitsComparer.Instance);
#else
            Array.Sort(allVariants, shader.VariantOffset, shader.VariantCount, OptionBitsComparer.Instance);
#endif
        }
    }

    private Span<ShaderSetFile.VariantHeader> VariantsOf(in ShaderSetFile.ShaderHeader shader) =>
#if NET5_0_OR_GREATER
        allVariants.Slice(shader.VariantOffset, shader.VariantCount);
#else
        allVariants.AsSpan(shader.VariantOffset, shader.VariantCount);
#endif

    public void LoadAll()
    {
        foreach (var shader in reader.Shaders)
        {
            var variants = VariantsOf(shader);
            for (int i = 0; i < variants.Length; i++)
            {
                var key = new ShaderVariantKey(shader.Info.SourceHash, variants[i].OptionBits);
                if (!loadedVariants.ContainsKey(key))
                    LoadVariant(key, variants[i]);
            }
        }
    }

    public void ClearLoaded() => loadedVariants.Clear();


    public bool TryGetShaderInfo(uint hash, [NotNullWhen(true)] out ShaderInfo? shaderInfo)
    {
        var shader = reader.Shaders.FirstOrDefault(s => s.Info.SourceHash == hash);
        shaderInfo = shader.Info;
        if (shaderInfo != null)
            return true;
        return FallbackShaderSet?.TryGetShaderInfo(hash, out shaderInfo) ?? false;
    }

    public bool TryGetShaderInfo(string name, [NotNullWhen(true)] out ShaderInfo? shaderInfo)
    {
        var shader = reader.Shaders.FirstOrDefault(s => s.Name == name);
        shaderInfo = shader.Info;
        if (shaderInfo != null)
            return true;
        return FallbackShaderSet?.TryGetShaderInfo(name, out shaderInfo) ?? false;
    }

    public bool TryGetSource(uint shaderHash, out string source)
    {
        source = "";
        if (shadersByHash.TryGetValue(shaderHash, out var index) && reader.Shaders[index].Source != null)
        {
            source = reader.Shaders[index].Source!;
            return true;
        }
        return FallbackShaderSet?.TryGetSource(shaderHash, out source) ?? false;
    }

    public bool TryGetVariant(ShaderVariantKey key, [NotNullWhen(true)] out ShaderVariant? variant)
    {
        variant = null;
        if (shadersByHash.TryGetValue(key.ShaderHash, out var shaderIndex))
        {
            var shader = reader.Shaders[shaderIndex];
            var searchVariant = new ShaderSetFile.VariantHeader()
            {
                Offset = 0u,
                OptionBits = key.OptionBits
            };
            var variantIndex = MemoryExtensions.BinarySearch(VariantsOf(shader), searchVariant, OptionBitsComparer.Instance);
            if (variantIndex >= 0)
            {
                variant = LoadVariant(key, VariantsOf(shader)[variantIndex]);
                return true;
            }
        }
        return FallbackShaderSet?.TryGetVariant(key, out variant) ?? false;
    }

    private ShaderVariant LoadVariant(ShaderVariantKey key, ShaderSetFile.VariantHeader variantHeader)
    {
        var variant = reader.ReadVariant(key.ShaderHash, variantHeader);
        loadedVariants[key] = variant;
        return variant;
    }

    public void Dispose()
    {
        ClearLoaded();
        reader.Dispose();
    }
}

internal sealed class OptionBitsComparer : IComparer<ShaderSetFile.VariantHeader>
{
    public static readonly OptionBitsComparer Instance = new();

    public int Compare(ShaderSetFile.VariantHeader x, ShaderSetFile.VariantHeader y) =>
        x.OptionBits.CompareTo(y.OptionBits);
}
