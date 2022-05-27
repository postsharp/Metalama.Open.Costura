// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder;

/// <summary>
///     Add <c>[assembly: DependencyEmbedderAspect]</c> anywhere in your source code to ensure that all references are
///     packed into
///     your main output assembly.
/// </summary>
[RequireAspectWeaver( "Metalama.Open.DependencyEmbedder.Weaver.DependencyEmbedderWeaver" )]
internal class DependencyEmbedderAspect : CompilationAspect { }