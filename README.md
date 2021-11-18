## Caravela.Open.DependencyEmbedder

Embeds dependencies as resources so that you can have a standalone executable.

This source transformer only works under .NET Framework. In .NET Core, we recommend upgrading to .NET Core 3 or later
and using [the single file executable feature](https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file)
instead.

*This is a [Caravela](https://github.com/postsharp/Caravela) aspect. It modifies your code during compilation by using
source weaving.*

[![CI badge](https://github.com/postsharp/Caravela.Open.DependencyEmbedder/workflows/Full%20Pipeline/badge.svg)](https://github.com/postsharp/Caravela.Open.DependencyEmbedder/actions?query=workflow%3A%22Full+Pipeline%22)

#### Example

Your project would normally result in `MyProject.exe` which requires `Newtonsoft.Json.dll` and `Soothsilver.Random.dll`
as dependencies because you used those NuGet packages.

If you use this source transformer, instead those two DLLs will be embedded into `MyProject.exe` as resources and loaded
from there.

#### Installation

1. Install the NuGet package: `dotnet add package Caravela.Open.DependencyEmbedder`.
2. Add the following code somewhere in your code:

    ```cs
    using Caravela.Framework.Fabrics;
    using Caravela.Open.DependencyEmbedder;
    
    internal class Fabric : ProjectFabric
    {
        public override void AmendProject(IProjectAmender amender)
        {
            amender.UseDependencyEmbedder();
        }
    }
    ```

You can then distribute just the main output assembly file. It will be enough.

There are documented configuration options in the `DependencyEmbedder` attribute. Set them in your source code to change
them from their defaults.