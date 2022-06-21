// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Metalama.Open.DependencyEmbedder.Tests;

public class BasicTest
{
#if DEBUG
    private const string _configuration = "Debug";
#else
    private const string _configuration = "Release";
#endif

    private readonly string _folder = Environment.CurrentDirectory;

    [Fact]
    public void TestTestAssemblyWithReferences()
    {
        
        var filename = $@"..\..\..\..\Metalama.Open.DependencyEmbedder.TestApp\bin\{_configuration}\net48\Metalama.Open.DependencyEmbedder.TestApp.exe";
        DeleteAllButExes( filename );
        var p = Process.Start( Path.Combine( this._folder, filename ) );
        Assert.True( p.WaitForExit( 5000 ) );
        Assert.Equal( 0, p.ExitCode );
    }

    [Fact]
    public void TestWpf()
    {
        
        var filename = $@"..\..\..\..\Metalama.Open.DependencyEmbedder.WpfApp\bin\{_configuration}\net48\Metalama.Open.DependencyEmbedder.WpfApp.exe";
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