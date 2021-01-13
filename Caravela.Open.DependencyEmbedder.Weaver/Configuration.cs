using Caravela.Framework.Sdk;
using Microsoft.CodeAnalysis;

namespace Caravela.Open.DependencyEmbedder.Weaver
{
    public static class Configuration
    {
        public static DependencyEmbedderAspect Read(AttributeData attributeData, IDiagnosticSink diagnosticSink)
        {
            DependencyEmbedderAspect config = new DependencyEmbedderAspect();
            var namedArguments = attributeData.NamedArguments;

            config.DisableCleanup = namedArguments.GetSafeBool(nameof(DependencyEmbedderAspect.DisableCleanup), false);
            config.DisableCompression = namedArguments.GetSafeBool(nameof(DependencyEmbedderAspect.DisableCompression), false);
            config.IncludeDebugSymbols = namedArguments.GetSafeBool(nameof(DependencyEmbedderAspect.IncludeDebugSymbols), true);
            config.CreateTemporaryAssemblies = namedArguments.GetSafeBool(nameof(DependencyEmbedderAspect.CreateTemporaryAssemblies), false);
            config.IgnoreSatelliteAssemblies = namedArguments.GetSafeBool(nameof(DependencyEmbedderAspect.IgnoreSatelliteAssemblies), false);
            
            config.IncludeAssemblies = namedArguments.GetSafeStringArray(nameof(DependencyEmbedderAspect.IncludeAssemblies));
            config.ExcludeAssemblies = namedArguments.GetSafeStringArray(nameof(DependencyEmbedderAspect.ExcludeAssemblies));
            config.PreloadOrder = namedArguments.GetSafeStringArray(nameof(DependencyEmbedderAspect.PreloadOrder));
            config.Unmanaged32Assemblies = namedArguments.GetSafeStringArray(nameof(DependencyEmbedderAspect.Unmanaged32Assemblies));
            config.Unmanaged64Assemblies = namedArguments.GetSafeStringArray(nameof(DependencyEmbedderAspect.Unmanaged64Assemblies));
            
            if (config.IncludeAssemblies != null && config.IncludeAssemblies.Length > 0 &&
                config.ExcludeAssemblies != null && config.ExcludeAssemblies.Length > 0)
            {
                var syntaxReference = attributeData.ApplicationSyntaxReference;
                diagnosticSink.AddDiagnostic(Diagnostic.Create(
                    "DE002", "Caravela.Open.DependencyEmbedder", "Set IncludeAssemblies, or ExcludeAssemblies, but not both.", DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0,
                    location: Location.Create(syntaxReference.SyntaxTree, syntaxReference.Span)));
            }
            return config;
        }
    }
}