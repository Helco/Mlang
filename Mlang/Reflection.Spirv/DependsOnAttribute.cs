using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlang.Reflection.Spirv;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class DependsOnAttribute : Attribute
{
    public string Version { get; init; } = "";
    public Capability[] Capabilities { get; init; } = Array.Empty<Capability>();
    public string[] Extensions { get; init; } = Array.Empty<string>();
}
