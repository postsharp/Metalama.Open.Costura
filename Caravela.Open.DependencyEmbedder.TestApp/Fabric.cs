using Caravela.Framework.Fabrics;
using Caravela.Open.DependencyEmbedder;

internal class Fabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        amender.UseDependencyEmbedder();
    }
}