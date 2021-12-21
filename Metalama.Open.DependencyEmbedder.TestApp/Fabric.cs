using Metalama.Framework.Fabrics;
using Metalama.Open.DependencyEmbedder;

internal class Fabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        amender.UseDependencyEmbedder();
    }
}