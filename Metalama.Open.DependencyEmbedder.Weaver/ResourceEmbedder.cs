using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Metalama.Compiler;
using Metalama.Framework.Impl.Sdk;

namespace Metalama.Open.DependencyEmbedder.Weaver
{
    public class ResourceEmbedder
    {
        private readonly AspectWeaverContext _context;
        private string? _cachePath;

        public bool HasUnmanaged { get; private set; }

        public List<(string Name, Stream Stream)> Resources { get; } = new();

        public ResourceEmbedder(AspectWeaverContext context)
        {
            _context = context;
        }

        public void EmbedResources(DependencyEmbedderOptions options, string[] referenceCopyLocalPaths,
            Checksums checksums)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            _cachePath = tempDirectory; //  Path.Combine(Path.GetDirectoryName(AssemblyFilePath), "DependencyEmbedder");
            Directory.CreateDirectory(_cachePath);

            var onlyBinaries = referenceCopyLocalPaths;

            var disableCompression = !options.CompressResources;
            var createTemporaryAssemblies = options.CreateTemporaryAssemblies;

            foreach (var dependency in GetFilteredReferences(onlyBinaries, options))
            {
                var fullPath = Path.GetFullPath(dependency);

                if (options.IncludeSatelliteAssemblies)
                    if (dependency.EndsWith(".resources.dll", StringComparison.OrdinalIgnoreCase))
                    {
                        Embed($"DependencyEmbedder.{Path.GetFileName(Path.GetDirectoryName(fullPath))}.", fullPath,
                            !disableCompression, createTemporaryAssemblies, options.IsCleanupDisabled, checksums);
                        continue;
                    }

                Embed("DependencyEmbedder.", fullPath, !disableCompression, createTemporaryAssemblies,
                    options.IsCleanupDisabled, checksums);

                if (!options.IncludeDebugSymbols) continue;
                var pdbFullPath = Path.ChangeExtension(fullPath, "pdb");
                if (File.Exists(pdbFullPath))
                    Embed("DependencyEmbedder.", pdbFullPath, !disableCompression, createTemporaryAssemblies,
                        options.IsCleanupDisabled, checksums);
            }

            foreach (var dependency in onlyBinaries)
            {
                var prefix = "";

                var unmanagedAssembly = options.UnmanagedAssemblies.LastOrDefault(x =>
                    string.Equals(x.Name, Path.GetFileNameWithoutExtension(dependency),
                        StringComparison.OrdinalIgnoreCase));
                if (unmanagedAssembly != null)
                {
                    if (unmanagedAssembly.Platform == UnmanagedAssemblyPlatform.x86)
                    {
                        prefix = "DependencyEmbedder32.";
                        HasUnmanaged = true;
                    }
                    else if (unmanagedAssembly.Platform == UnmanagedAssemblyPlatform.x64)
                    {
                        prefix = "DependencyEmbedder64.";
                        HasUnmanaged = true;
                    }
                }

                if (string.IsNullOrEmpty(prefix)) continue;

                var fullPath = Path.GetFullPath(dependency);
                Embed(prefix, fullPath, !disableCompression, true, options.IsCleanupDisabled, checksums);

                if (!options.IncludeDebugSymbols) continue;
                var pdbFullPath = Path.ChangeExtension(fullPath, "pdb");
                if (File.Exists(pdbFullPath))
                    Embed(prefix, pdbFullPath, !disableCompression, true, options.IsCleanupDisabled, checksums);
            }
        }

        private bool CompareAssemblyName(string matchText, string assemblyName)
        {
            if (matchText.EndsWith("*") && matchText.Length > 1)
                return assemblyName.StartsWith(matchText.Substring(0, matchText.Length - 1),
                    StringComparison.OrdinalIgnoreCase);

            return matchText.Equals(assemblyName, StringComparison.OrdinalIgnoreCase);
        }

        private IEnumerable<string> GetFilteredReferences(IEnumerable<string> onlyBinaries,
            DependencyEmbedderOptions options)
        {
            if (!options.IncludedAssemblies.IsDefault)
            {
                var skippedAssemblies = new List<string>(options.IncludedAssemblies);

                foreach (var file in onlyBinaries)
                {
                    var assemblyName = Path.GetFileNameWithoutExtension(file);

                    if (options.IncludedAssemblies.Any(x => CompareAssemblyName(x, assemblyName)) &&
                        options.UnmanagedAssemblies.All(x => !CompareAssemblyName(x.Name, assemblyName)))
                    {
                        skippedAssemblies.Remove(
                            options.IncludedAssemblies.First(x => CompareAssemblyName(x, assemblyName)));
                        yield return file;
                    }
                }

                if (skippedAssemblies.Count > 0)
                {
                    var splitReferences = Array.Empty<string>(); // References.Split(';');

                    var hasErrors = false;

                    foreach (var skippedAssembly in skippedAssemblies)
                    {
                        var fileName = (from splitReference in splitReferences
                            where string.Equals(Path.GetFileNameWithoutExtension(splitReference), skippedAssembly,
                                StringComparison.InvariantCulture)
                            select splitReference).FirstOrDefault();
                        if (string.IsNullOrEmpty(fileName))
                        {
                            hasErrors = true;
                            // TODO  LogError($"Assembly '{skippedAssembly}' cannot be found (not even as CopyLocal='false'), please update the configuration");
                            continue;
                        }

                        yield return fileName;
                    }

                    if (hasErrors) throw new Exception("One or more errors occurred, please check the log");
                }

                yield break;
            }

            if (options.ExcludedAssemblies.Any())
            {
                foreach (var file in onlyBinaries.Except(options.UnmanagedAssemblies.Select(x => x.Name)))
                {
                    var assemblyName = Path.GetFileNameWithoutExtension(file);

                    if (options.ExcludedAssemblies.Any(x => CompareAssemblyName(x, assemblyName)) ||
                        options.UnmanagedAssemblies.Any(x => CompareAssemblyName(x.Name, assemblyName)))
                        continue;
                    yield return file;
                }

                yield break;
            }

            if (options.IncludedAssemblies.IsDefault)
                foreach (var file in onlyBinaries)
                {
                    var assemblyName = Path.GetFileNameWithoutExtension(file);

                    if (options.UnmanagedAssemblies.All(x => !CompareAssemblyName(x.Name, assemblyName)))
                        yield return file;
                }
        }

        private void Embed(string prefix, string fullPath, bool compress, bool addChecksum, bool disableCleanup,
            Checksums checksums)
        {
            try
            {
                InnerEmbed(prefix, fullPath, compress, addChecksum, disableCleanup, checksums);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    innerException: exception,
                    message: $@"Failed to embed.
prefix: {prefix}
fullPath: {fullPath}
compress: {compress}
addChecksum: {addChecksum}
disableCleanup: {disableCleanup}");
            }
        }

        private void InnerEmbed(string prefix, string fullPath, bool compress, bool addChecksum, bool disableCleanup,
            Checksums checksums)
        {
            if (!disableCleanup)
            {
                // in any case we can remove this from the copy local paths, because either it's already embedded, or it will be embedded.
                // ReferenceCopyLocalPaths.RemoveAll(item => string.Equals(item, fullPath, StringComparison.OrdinalIgnoreCase));
            }

            var resourceName = $"{prefix}{Path.GetFileName(fullPath).ToLowerInvariant()}";

            if (compress) resourceName += ".compressed";

            var checksum = Checksums.CalculateChecksum(fullPath);
            var cacheFile = Path.Combine(_cachePath, $"{checksum}.{resourceName}");
            var memoryStream = BuildMemoryStream(fullPath, compress, cacheFile);
            Resources.Add((resourceName, memoryStream));
            _context.AddResource(new ManagedResource(resourceName, memoryStream.GetBuffer()));

            if (addChecksum) checksums.Add(resourceName, checksum);
        }

        private static MemoryStream BuildMemoryStream(string fullPath, bool compress, string cacheFile)
        {
            var memoryStream = new MemoryStream();

            if (File.Exists(cacheFile))
            {
                using var fileStream = File.Open(cacheFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fileStream.CopyTo(memoryStream);
            }
            else
            {
                using var cacheFileStream = File.Open(cacheFile, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
                using var fileStream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                if (compress)
                {
                    using var compressedStream = new DeflateStream(memoryStream, CompressionMode.Compress, true);
                    fileStream.CopyTo(compressedStream);
                }
                else
                {
                    fileStream.CopyTo(memoryStream);
                }

                memoryStream.Position = 0;
                memoryStream.CopyTo(cacheFileStream);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}