using System;
using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;

namespace Metalama.Open.DependencyEmbedder
{
    [CompileTimeOnly]
    public static class DependencyEmbedderProjectExtensions
    {
        public static void UseDependencyEmbedder(this IProjectAmender projectAmender,
            Action<DependencyEmbedderOptions>? configure = null)
        {
            var options = projectAmender.Project.Extension<DependencyEmbedderOptions>();
            if (configure != null) configure(options);

            projectAmender.WithTargetMembers(c => new[] { c }).AddAspect(_ => new DependencyEmbedderAspect());
        }
    }
}