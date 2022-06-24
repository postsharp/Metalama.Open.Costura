// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Compiler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Metalama.Open.Costura.Weaver;

internal static class ResourceHash
{
    public static string CalculateHash( List<ManagedResource> resources )
    {
        var data = resources
            .OrderBy( r => r.Name )
            .Where( r => r.Name.StartsWith( "Costura", StringComparison.Ordinal ) )
            .Select( r => r.DataProvider!.Invoke() )
            .ToArray();

        var allStream = new ConcatenatedStream( data );

#pragma warning disable CA5351
        using var md5 = MD5.Create();
#pragma warning restore CA5351
        var hashBytes = md5.ComputeHash( allStream );

        var sb = new StringBuilder();

        foreach ( var t in hashBytes )
        {
            sb.Append( t.ToString( "X2", CultureInfo.InvariantCulture ) );
        }

        allStream.ResetAllToZero();

        return sb.ToString();
    }
}