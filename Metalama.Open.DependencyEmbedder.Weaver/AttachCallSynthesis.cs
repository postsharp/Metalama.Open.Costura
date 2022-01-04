using Microsoft.CodeAnalysis.CSharp;

namespace Metalama.Open.DependencyEmbedder.Weaver
{
    public class AttachCallSynthesis
    {
        public void SynthesizeCallToAttach(ref CSharpCompilation compilation, CSharpParseOptions parseOptions,
            AssemblyLoaderInfo assemblyLoaderInfo)
        {
            var code = Resources.ModuleInitializer.Replace("TEMPLATE", assemblyLoaderInfo.SourceTypeName);

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code, parseOptions));
        }
    }
}