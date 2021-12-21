using System;
using System.Collections.Immutable;
using Caravela.Framework.Project;

namespace Caravela.Open.DependencyEmbedder
{
    public class DependencyEmbedderOptions : ProjectExtension
    {
        private bool _includeDebugSymbols;
        private bool _useCompression = true;
        private bool _disableCleanup;
        private bool _createTemporaryAssemblies;
        private bool _includeSatelliteAssemblies = true;
        private ImmutableArray<string> _includedAssemblies;
        private ImmutableArray<string> _excludedAssemblies = ImmutableArray<string>.Empty;
        private ImmutableArray<UnmanagedAssembly> _unmanagedAssemblies = ImmutableArray<UnmanagedAssembly>.Empty;
        private ImmutableArray<string> _preloadOrder;

        private void CheckWritable()
        {
            if (IsReadOnly)
                throw new InvalidOperationException(
                    "Cannot modify this DependencyEmbedderOptions because it is read-only.");
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>.pdb</c> files should also be embedded. The default value is <c>true</c>.
        /// </summary>
        public bool IncludeDebugSymbols
        {
            get => _includeDebugSymbols;

            set
            {
                CheckWritable();
                _includeDebugSymbols = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether resources embedded into the main assembly should be compressed. The default value is <c>true</c>.
        /// </summary>
        public bool CompressResources
        {
            get => _useCompression;

            set
            {
                CheckWritable();
                _useCompression = value;
            }
        }

        /// <summary>
        /// This option doesn't work. If it did, it would control whether
        /// embedded assemblies are placed in the output folder anyway, even
        /// though they aren't necessary anymore.
        /// </summary>
        public bool IsCleanupDisabled
        {
            get => _disableCleanup;
            set
            {
                CheckWritable();
                _disableCleanup = value;
            }
        }

        /// <summary>
        /// If true, then Packer will bootstrap itself in your assembly's module initializer and you don't need to
        /// call <see cref="PackerUtility.Initialize"/>. Default true ("load automatically").
        /// </summary>
        //public bool LoadAtModuleInit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether embedded assemblies should be copied to disk before loading them into
        /// memory. This is helpful for some scenarios that expected an assembly to be loaded from a physical file. 
        /// For example, if some code checks the assembly's assembly location. The default value is <c>false</c>.
        /// </summary>
        public bool CreateTemporaryAssemblies
        {
            get => _createTemporaryAssemblies;

            set
            {
                CheckWritable();
                _createTemporaryAssemblies = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether satellite assemblies (typically containing localized resources,
        /// i.e. with a name like 'resources.dll') should be included, The default value is <c>true</c>.
        /// </summary>
        public bool IncludeSatelliteAssemblies
        {
            get => _includeSatelliteAssemblies;
            set
            {
                CheckWritable();
                _includeSatelliteAssemblies = value;
            }
        }

        /// <summary>
        /// Gets or sets the lists of assembly names to embed. The assembly name should not include the <c>.exe</c> or  <c>.dll</c> extension.
        /// The names can include wildcards at the end of the name for partial matching. For example <c>System.*</c> will exclude all assemblies that start with System..
        /// By default, all references with Copy Local set to <c>true</c> are embedded.
        /// </summary>
        public ImmutableArray<string> IncludedAssemblies
        {
            get => _includedAssemblies;

            set
            {
                CheckWritable();
                _includedAssemblies = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of assembly names to exclude from embedding. The assembly name should not include the <c>.exe</c> or  <c>.dll</c> extension.
        /// The names can include wildcards at the end of the name for partial assembly name matching. For example <c>System.*</c> will exclude all assemblies that start with System..
        /// </summary>
        public ImmutableArray<string> ExcludedAssemblies
        {
            get => _excludedAssemblies;

            set
            {
                CheckWritable();
                _excludedAssemblies = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of mixed-mode assemblies. These assemblies must be loaded differently than the other.
        /// </summary>
        public ImmutableArray<UnmanagedAssembly> UnmanagedAssemblies
        {
            get => _unmanagedAssemblies;

            set
            {
                CheckWritable();
                _unmanagedAssemblies = value;
            }
        }


        /// <summary>
        /// Native libraries can be loaded by this add-in automatically.
        /// To include a native library include it in your project as an
        /// Embedded Resource in a folder called DependencyEmbedder32 or DependencyEmbedder64
        /// depending on the bittyness of the library.
        /// Optionally you can also specify the order that preloaded
        /// libraries are loaded. When using temporary assemblies
        /// from disk mixed mode assemblies are also preloaded.
        /// </summary>
        public ImmutableArray<string> PreloadOrder
        {
            get => _preloadOrder;
            set
            {
                CheckWritable();
                _preloadOrder = value;
            }
        }
    }
}