using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder
{
    [CompileTimeOnly]
    public class UnmanagedAssembly
    {
        public string Name { get; }
        public UnmanagedAssemblyPlatform Platform { get; }

        public UnmanagedAssembly(string name, UnmanagedAssemblyPlatform platform)
        {
            Name = name;
            Platform = platform;
        }
    }
}