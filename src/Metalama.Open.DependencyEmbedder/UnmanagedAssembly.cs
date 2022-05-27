// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder;

[CompileTime]
public class UnmanagedAssembly
{
    public UnmanagedAssembly( string name, UnmanagedAssemblyPlatform platform )
    {
        this.Name = name;
        this.Platform = platform;
    }

    public string Name { get; }

    public UnmanagedAssemblyPlatform Platform { get; }
}