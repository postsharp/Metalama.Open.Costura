// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder;

[CompileTime]
public enum UnmanagedAssemblyPlatform
{
    X86,
    X64
}