// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;
using System;

namespace Metalama.Open.DependencyEmbedder;

[CompileTime]
public static class DependencyEmbedderProjectExtensions
{
    public static void UseDependencyEmbedder(
        this IProjectAmender projectAmender,
        Action<DependencyEmbedderOptions>? configure = null )
    {
        var options = projectAmender.Project.Extension<DependencyEmbedderOptions>();

        configure?.Invoke( options );

        projectAmender.With( c => c ).AddAspect<DependencyEmbedderAspect>();
    }
}