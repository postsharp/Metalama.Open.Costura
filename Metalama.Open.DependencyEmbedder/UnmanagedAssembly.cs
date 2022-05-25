// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder;

[CompileTime]
public class UnmanagedAssembly
{
    public UnmanagedAssembly( string name, UnmanagedAssemblyPlatform platform )
    {
        this.Name = name;
        this.Platform = platform;
    }

    public string Name { get; }

    public UnmanagedAssemblyPlatform Platform { get; }
}