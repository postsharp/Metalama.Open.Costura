// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Newtonsoft.Json;
using Soothsilver.Random;
using System;
using Xunit;

namespace Metalama.Open.DependencyEmbedder.TestApp;

internal class ThenUse
{
    public static void Stuff()
    {
        var serializeObject = JsonConvert.SerializeObject( new[] { "he", "ha" } );
        var r = serializeObject + R.Next( 0, 1 );
        Assert.Equal( @"[""he"",""ha""]0", r );
        Console.WriteLine( "This is still working: " + r );
    }
}