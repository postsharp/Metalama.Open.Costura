using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder
{
    /// <summary>
    /// Add <c>[assembly: DependencyEmbedderAspect]</c> anywhere in your source code to ensure that all references are packed into
    /// your main output assembly.
    /// </summary>
    internal class DependencyEmbedderAspect : CompilationAspect
    {
    }
}