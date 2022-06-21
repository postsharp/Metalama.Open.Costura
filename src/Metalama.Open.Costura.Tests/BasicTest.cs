// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Metalama.Open.Costura.Tests;

public class BasicTest
{
    private readonly string _folder = Environment.CurrentDirectory;

    [Fact]
    public void TestTestAssemblyWithReferences()
    {
        
        var filename = @"..\..\..\..\Metalama.Open.Costura.TestApp\bin\Debug\net48\Metalama.Open.Costura.TestApp.exe";
        DeleteAllButExes( filename );
        var p = Process.Start( Path.Combine( this._folder, filename ) );
        Assert.True( p.WaitForExit( 5000 ) );
        Assert.Equal( 0, p.ExitCode );
    }

    [Fact]
    public void TestWpf()
    {
        
        var filename = @"..\..\..\..\Metalama.Open.Costura.WpfApp\bin\Debug\net48\Metalama.Open.Costura.WpfApp.exe";
        DeleteAllButExes( filename );
        var p = Process.Start( Path.Combine( this._folder, filename ) );
        Assert.True( p.WaitForExit( 35000 ) );
        Assert.Equal( 0, p.ExitCode );
    }

    private static void DeleteAllButExes( string file )
    {
        foreach ( var filename in Directory.EnumerateFiles( Path.GetDirectoryName(file)! ).ToList() )
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