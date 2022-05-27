// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Metalama.Open.DependencyEmbedder.Tests;

public class BasicTest
{
    private readonly string _folder = Environment.CurrentDirectory;

    [Fact]
    public void TestTestAssemblyWithReferences()
    {
        DeleteAllButExes( this._folder );
        var filename = "Metalama.Open.DependencyEmbedder.TestApp.exe";
        var p = Process.Start( Path.Combine( this._folder, filename ) );
        Assert.True( p.WaitForExit( 5000 ) );
        Assert.Equal( 0, p.ExitCode );
    }

    [Fact]
    public void TestWpf()
    {
        DeleteAllButExes( this._folder );
        var filename = "Metalama.Open.DependencyEmbedder.WpfApp.exe";
        var p = Process.Start( Path.Combine( this._folder, filename ) );
        Assert.True( p.WaitForExit( 35000 ) );
        Assert.Equal( 0, p.ExitCode );
    }

    private static void DeleteAllButExes( string folder )
    {
        foreach ( var filename in Directory.EnumerateFiles( folder ).ToList() )
        {
            if ( filename.EndsWith( ".exe", StringComparison.OrdinalIgnoreCase ) )
            {
                // keep
            }
            else
            {
                File.Delete( filename );
            }
        }
    }
}