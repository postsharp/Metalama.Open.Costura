// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder;

[CompileTime]
public enum UnmanagedAssemblyPlatform
{
    X86,
    X64
}