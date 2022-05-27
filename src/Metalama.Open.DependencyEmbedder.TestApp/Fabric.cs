// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Fabrics;

namespace Metalama.Open.DependencyEmbedder.TestApp;

internal class Fabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender.UseDependencyEmbedder();
    }
}