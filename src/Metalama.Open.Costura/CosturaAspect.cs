// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;

namespace Metalama.Open.Costura;

/// <summary>
///     Add <c>[assembly: CosturaAspect]</c> anywhere in your source code to ensure that all references are
///     packed into
///     your main output assembly.
/// </summary>
[RequireAspectWeaver( "Metalama.Open.Costura.Weaver.CosturaWeaver" )]
internal class CosturaAspect : CompilationAspect { }