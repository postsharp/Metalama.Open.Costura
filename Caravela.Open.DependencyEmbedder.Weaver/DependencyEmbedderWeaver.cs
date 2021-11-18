using System;
using System.IO;
using System.Linq;
using Caravela.Framework.Impl.CodeModel;
using Caravela.Framework.Impl.Sdk;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Caravela.Open.DependencyEmbedder.Weaver
{
    [CompilerPlugin]
    [AspectWeaver(typeof(DependencyEmbedderAspect))]
    public class DependencyEmbedderWeaver : IAspectWeaver
    {
        public void Transform(AspectWeaverContext context)
        {
            var compilation = (CSharpCompilation)context.Compilation.Compilation;

            // Check the language version.
            if (compilation.LanguageVersion < LanguageVersion.CSharp9)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "DE001", "Language version too low",
                        "Caravela.Open.DependencyEmbedder requires language version at least 9.0, but it's set to {0}.",
                        "Caravela.Open.DependencyEmbedder", DiagnosticSeverity.Error, true),
                    null, new object[] { compilation.LanguageVersion.ToDisplayString() }));
                return;
            }

            var options = context.Project.Extension<DependencyEmbedderOptions>();

            var excludedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                @"Reference Assemblies\Microsoft\Framework\.NETFramework");

            var paths = compilation.References.Select(r => r switch
                {
                    PortableExecutableReference peReference => peReference.FilePath,
                    _ => throw new NotSupportedException()
                })
                .Where(path => path != null && !path.StartsWith(excludedPath))
                .ToArray();

            var parseOptions = new CSharpParseOptions(compilation.LanguageVersion);

            // Embed resources:
            var checksums = new Checksums();
            var resourceEmbedder = new ResourceEmbedder(context);
            resourceEmbedder.EmbedResources(options, paths!, checksums);
            var unmanagedFromEmbedder = resourceEmbedder.HasUnmanaged;

            // Load references:
            var info = AssemblyLoaderInfo.LoadAssemblyLoader(options.CreateTemporaryAssemblies, unmanagedFromEmbedder,
                ref compilation, parseOptions);

            // Alter code:
            var resourcesHash = ResourceHash.CalculateHash(resourceEmbedder.Resources);
            new AttachCallSynthesis().SynthesizeCallToAttach(ref compilation, parseOptions, info);
            new ResourceNameFinder(info, resourceEmbedder.Resources.Select(r => r.Name)).FillInStaticConstructor(
                options.CreateTemporaryAssemblies,
                options.PreloadOrder,
                resourcesHash,
                checksums);


            context.Compilation =
                context.Compilation.AddSyntaxTrees(SyntaxFactory.SyntaxTree(info.SourceType, parseOptions));
        }
    }
}