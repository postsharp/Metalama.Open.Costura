using System;
using Caravela.Framework.Aspects;
using Caravela.Framework.Fabrics;

namespace Caravela.Open.DependencyEmbedder
{
    [CompileTimeOnly]
    public static class DependencyEmbedderProjectExtensions
    {
        public static void UseDependencyEmbedder(this IProjectAmender projectAmender,
            Action<DependencyEmbedderOptions>? configure = null)
        {
            var options = projectAmender.Project.Extension<DependencyEmbedderOptions>();
            if (configure != null) configure(options);

            projectAmender.WithMembers(c => new[] { c }).AddAspect(_ => new DependencyEmbedderAspect());
        }
    }
}