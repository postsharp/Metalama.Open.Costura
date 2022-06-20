// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;
using System;

namespace Metalama.Open.Costura;

[CompileTime]
public static class CosturaProjectExtensions
{
    public static void UseCostura(
        this IProjectAmender projectAmender,
        Action<CosturaOptions>? configure = null )
    {
        var options = projectAmender.Project.Extension<CosturaOptions>();

        configure?.Invoke( options );

        projectAmender.With( c => c ).AddAspect<CosturaAspect>();
    }
}