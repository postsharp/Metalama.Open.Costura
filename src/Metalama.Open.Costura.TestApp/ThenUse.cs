// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Newtonsoft.Json;
using Soothsilver.Random;
using System;
using Xunit;

namespace Metalama.Open.Costura.TestApp;

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