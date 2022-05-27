// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Metalama.Framework.Aspects;

namespace Metalama.Open.DependencyEmbedder;

/// <summary>
///     Add <c>[assembly: DependencyEmbedderAspect]</c> anywhere in your source code to ensure that all references are
///     packed into
///     your main output assembly.
/// </summary>
[RequireAspectWeaver( "Metalama.Open.DependencyEmbedder.Weaver.DependencyEmbedderWeaver" )]
internal class DependencyEmbedderAspect : CompilationAspect { }