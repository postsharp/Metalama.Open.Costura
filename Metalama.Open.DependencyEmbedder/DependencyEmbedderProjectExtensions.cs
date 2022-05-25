// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;
using System;

namespace Metalama.Open.DependencyEmbedder;

[CompileTime]
public static class DependencyEmbedderProjectExtensions
{
    public static void UseDependencyEmbedder(
        this IProjectAmender projectAmender,
        Action<DependencyEmbedderOptions>? configure = null )
    {
        var options = projectAmender.Project.Extension<DependencyEmbedderOptions>();

        configure?.Invoke( options );

        projectAmender.With( c => c ).AddAspect<DependencyEmbedderAspect>();
    }
}