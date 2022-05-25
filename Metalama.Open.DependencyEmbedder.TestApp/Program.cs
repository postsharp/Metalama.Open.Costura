// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

namespace Metalama.Open.DependencyEmbedder.TestApp;

internal class Program
{
    private static void Main()
    {
        Delay();
    }

    private static void Delay()
    {
        ThenUse.Stuff();
    }
}