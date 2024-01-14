using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mlang.Model;

public enum ScalarWidth
{
    DWord,
    Word,
    Byte
}

public enum ScalarType
{
    Int,
    UInt,
    Float
}

public static partial class NumericTypeExtensions
{
    public static string AsPrefix(this ScalarType type) => type switch
    {
        ScalarType.Int => "i",
        ScalarType.UInt => "u",
        _ => ""
    };
}

public readonly record struct NumericType(
    ScalarType Scalar,
    int Columns = 1,
    int Rows = 1,
    ScalarWidth ScalarWidth = ScalarWidth.DWord,
    bool IsNormalized = false
) : IDataType
{
    public bool IsInvalid =>
        Rows is < 1 or > 4 ||
        Columns is < 1 or > 4 ||
        Scalar == ScalarType.Float && ScalarWidth != ScalarWidth.DWord ||
        IsMatrix && Scalar != ScalarType.Float;

    public bool IsScalar => Rows == 1 && Columns == 1;
    public bool IsVector => Rows > 1 && Columns == 1;
    public bool IsMatrix => Columns != 1;
    public bool IsGLSLCompatible => TryGetGLSLCompatible(out var compatible) && compatible == this;
    
    public bool TryGetGLSLCompatible(out NumericType compatible)
    {
        compatible = default;
        if (IsInvalid)
            return false;
        compatible = this with
        {
            Scalar =
                Scalar == ScalarType.Float || IsNormalized ? ScalarType.Float :
                Scalar == ScalarType.Int ? ScalarType.Int
                : ScalarType.UInt,
            ScalarWidth = ScalarWidth.DWord,
            IsNormalized = false,
        };
        return true;
    }

    public string MlangName
    {
        get
        {
            if (IsInvalid)
                return "<invalid-numeric-type>";
            var name = new StringBuilder(8);
            if (Scalar == ScalarType.Float)
                name.Append("float");
            else if (ScalarWidth == ScalarWidth.Byte)
                name.Append(Scalar == ScalarType.Int ? "sbyte" : "byte");
            else
            {
                if (Scalar == ScalarType.UInt)
                    name.Append('u');
                name.Append(ScalarWidth == ScalarWidth.DWord ? "int" : "short");
            }
            if (IsVector)
                name.Append(Rows);
            else if (IsMatrix)
            {
                name.Append(Columns);
                name.Append('x');
                name.Append(Rows);
            }
            if (IsNormalized)
                name.Append("_norm");
            return name.ToString();
        }
    }

    public string GLSLName
    {
        get
        {
            if (!TryGetGLSLCompatible(out var compatible))
                throw new InvalidOperationException("Given numeric type is not GLSL compatible");
            if (compatible.IsScalar)
                return MlangName;
            else if (compatible.IsVector)
            {
                var name = new StringBuilder(8);
                name.Append(compatible.Scalar.AsPrefix());
                name.Append("vec");
                name.Append(Rows);
                return name.ToString();
            }
            else // IsMatrix
            {
                var name = new StringBuilder(8);
                name.Append("mat");
                name.Append(Columns);
                if (Rows != Columns)
                {
                    name.Append('x');
                    name.Append(Rows);
                }
                return name.ToString();
            }
        }
    }

    private int SizeOfVector => (Rows == 3 ? 4 : Rows) * sizeof(float);
    public int Size => Columns * SizeOfVector;
    public int Alignment => SizeOfVector;

    public override string ToString() => MlangName;

    public static bool TryParse(string name, out NumericType type) =>
        TypeNames.TryGetValue(name, out type);

    private static readonly IReadOnlyDictionary<string, NumericType> TypeNames = new Dictionary<string, NumericType>()
    {
        { "sbyte", new(ScalarType.Int, 1, 1, ScalarWidth: ScalarWidth.Byte)},
        { "sbyte1", new(ScalarType.Int, 1, 1, ScalarWidth: ScalarWidth.Byte)},
        { "sbyte2", new(ScalarType.Int, 1, 2, ScalarWidth: ScalarWidth.Byte)},
        { "sbyte3", new(ScalarType.Int, 1, 3, ScalarWidth: ScalarWidth.Byte)},
        { "sbyte4", new(ScalarType.Int, 1, 4, ScalarWidth: ScalarWidth.Byte)},
        { "byte", new(ScalarType.UInt, 1, 1, ScalarWidth: ScalarWidth.Byte)},
        { "byte1", new(ScalarType.UInt, 1, 1, ScalarWidth: ScalarWidth.Byte)},
        { "byte2", new(ScalarType.UInt, 1, 2, ScalarWidth: ScalarWidth.Byte)},
        { "byte3", new(ScalarType.UInt, 1, 3, ScalarWidth: ScalarWidth.Byte)},
        { "byte4", new(ScalarType.UInt, 1, 4, ScalarWidth: ScalarWidth.Byte)},
        { "short", new(ScalarType.Int, 1, 1, ScalarWidth: ScalarWidth.Word)},
        { "short1", new(ScalarType.Int, 1, 1, ScalarWidth: ScalarWidth.Word)},
        { "short2", new(ScalarType.Int, 1, 2, ScalarWidth: ScalarWidth.Word)},
        { "short3", new(ScalarType.Int, 1, 3, ScalarWidth: ScalarWidth.Word)},
        { "short4", new(ScalarType.Int, 1, 4, ScalarWidth: ScalarWidth.Word)},
        { "ushort", new(ScalarType.UInt, 1, 1, ScalarWidth: ScalarWidth.Word)},
        { "ushort1", new(ScalarType.UInt, 1, 1, ScalarWidth: ScalarWidth.Word)},
        { "ushort2", new(ScalarType.UInt, 1, 2, ScalarWidth: ScalarWidth.Word)},
        { "ushort3", new(ScalarType.UInt, 1, 3, ScalarWidth: ScalarWidth.Word)},
        { "ushort4", new(ScalarType.UInt, 1, 4, ScalarWidth: ScalarWidth.Word)},
        { "int", new(ScalarType.Int, 1, 1)},
        { "int1", new(ScalarType.Int, 1, 1)},
        { "int2", new(ScalarType.Int, 1, 2)},
        { "int3", new(ScalarType.Int, 1, 3)},
        { "int4", new(ScalarType.Int, 1, 4)},
        { "uint", new(ScalarType.UInt, 1, 1)},
        { "uint1", new(ScalarType.UInt, 1, 1)},
        { "uint2", new(ScalarType.UInt, 1, 2)},
        { "uint3", new(ScalarType.UInt, 1, 3)},
        { "uint4", new(ScalarType.UInt, 1, 4)},
        { "float", new(ScalarType.Float, 1, 1)},
        { "float1", new(ScalarType.Float, 1, 1)},
        { "float2", new(ScalarType.Float, 1, 2)},
        { "float3", new(ScalarType.Float, 1, 3)},
        { "float4", new(ScalarType.Float, 1, 4)},

        { "sbyte2_norm", new(ScalarType.Int, 1, 2, ScalarWidth: ScalarWidth.Byte, IsNormalized: true)},
        { "sbyte4_norm", new(ScalarType.Int, 1, 4, ScalarWidth: ScalarWidth.Byte, IsNormalized: true)},
        { "byte2_norm", new(ScalarType.UInt, 1, 2, ScalarWidth: ScalarWidth.Byte, IsNormalized: true)},
        { "byte4_norm", new(ScalarType.UInt, 1, 4, ScalarWidth: ScalarWidth.Byte, IsNormalized: true)},
        { "short2_norm", new(ScalarType.Int, 1, 2, ScalarWidth: ScalarWidth.Word, IsNormalized: true)},
        { "short4_norm", new(ScalarType.Int, 1, 4, ScalarWidth: ScalarWidth.Word, IsNormalized: true)},
        { "ushort2_norm", new(ScalarType.UInt, 1, 2, ScalarWidth: ScalarWidth.Word, IsNormalized: true)},
        { "ushort4_norm", new(ScalarType.UInt, 1, 4, ScalarWidth: ScalarWidth.Word, IsNormalized: true)},
        { "int2_norm", new(ScalarType.Int, 1, 2, IsNormalized: true)},
        { "int4_norm", new(ScalarType.Int, 1, 4, IsNormalized: true)},
        { "uint2_norm", new(ScalarType.UInt, 1, 2, IsNormalized: true)},
        { "uint4_norm", new(ScalarType.UInt, 1, 4, IsNormalized: true)},

        { "vec2", new(ScalarType.Float, 1, 2)},
        { "vec3", new(ScalarType.Float, 1, 3)},
        { "vec4", new(ScalarType.Float, 1, 4)},
        { "ivec2", new(ScalarType.Int, 1, 2)},
        { "ivec3", new(ScalarType.Int, 1, 3)},
        { "ivec4", new(ScalarType.Int, 1, 4)},
        { "uvec2", new(ScalarType.UInt, 1, 2)},
        { "uvec3", new(ScalarType.UInt, 1, 3)},
        { "uvec4", new(ScalarType.UInt, 1, 4)},

        { "mat2", new(ScalarType.Float, 2, 2)},
        { "mat3", new(ScalarType.Float, 3, 3)},
        { "mat4", new(ScalarType.Float, 4, 4)},
        { "mat2x2", new(ScalarType.Float, 2, 2)},
        { "mat2x3", new(ScalarType.Float, 2, 3)},
        { "mat2x4", new(ScalarType.Float, 2, 4)},
        { "mat3x2", new(ScalarType.Float, 3, 2)},
        { "mat3x3", new(ScalarType.Float, 3, 3)},
        { "mat3x4", new(ScalarType.Float, 3, 4)},
        { "mat4x2", new(ScalarType.Float, 4, 2)},
        { "mat4x3", new(ScalarType.Float, 4, 3)},
        { "mat4x4", new(ScalarType.Float, 4, 4)},
    };

    DataTypeCategory IDataType.Category => DataTypeCategory.Numeric;

    void IDataType.Write(BinaryWriter writer)
    {
        writer.Write((byte)Scalar);
        writer.Write((byte)Columns);
        writer.Write((byte)Rows);
        writer.Write((byte)ScalarWidth);
        writer.Write(IsNormalized);
    }
    internal void Write(BinaryWriter writer) => (this as IDataType).Write(writer);

    internal static NumericType Read(BinaryReader reader) => new(
        (ScalarType)reader.ReadByte(),
        reader.ReadByte(),
        reader.ReadByte(),
        (ScalarWidth)reader.ReadByte(),
        reader.ReadBoolean());
}
