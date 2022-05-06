// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using PostSharp.Engineering.BuildTools;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Build.Solutions;
using PostSharp.Engineering.BuildTools.Dependencies.Model;
using Spectre.Console.Cli;

var productDependencyDefinition = new DependencyDefinition( "Metalama.Open.DependencyEmbedder", VcsProvider.GitHub, "Metalama" )
{
    CiBuildTypes = new ConfigurationSpecific<string>(
        "Metalama_MetalamaOpen_MetalamaOpenDependencyEmbedder_DebugBuild",
        "Metalama_MetalamaOpen_MetalamaOpenDependencyEmbedder_ReleaseBuild",
        "Metalama_MetalamaOpen_MetalamaOpenDependencyEmbedder_PublicBuild" ),
    DeploymentBuildType = "Metalama_MetalamaOpen_MetalamaOpenDependencyEmbedder_PublicDeployment",
    BumpBuildType = "Metalama_MetalamaOpen_MetalamaOpenDependencyEmbedder_VersionBump"
};

var product = new Product( productDependencyDefinition )
{
    Solutions = new Solution[] { new DotNetSolution( "Metalama.Open.DependencyEmbedder.sln" ) },
    PublicArtifacts = Pattern.Create( "Metalama.Open.DependencyEmbedder.$(PackageVersion).nupkg" ),
    Dependencies = new[] { Dependencies.PostSharpEngineering, Dependencies.Metalama },
    RequiresBranchMerging = true
};

var commandApp = new CommandApp();

commandApp.AddProductCommands( product );

return commandApp.Run( args );