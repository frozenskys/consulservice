#addin "nuget:?package=NuGet.Core"
#addin "nuget:?package=Cake.ExtendedNuGet"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifacts = MakeAbsolute(Directory(Argument("artifactPath", "./artifacts")));
var buildFolder = MakeAbsolute(Directory(Argument("buildFolder", "./build"))).ToString();

///////////////////////////////////////////////////////////////////////////////
// USER TASKS
// PUT ALL YOUR BUILD GOODNESS IN HERE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
	{
		CleanDirectory(artifacts);
		CleanDirectory(buildFolder);
	});

Task("Default")
    .IsDependentOn("Package")
    .Does(() => 
	{
	});

Task("Restore")
	.IsDependentOn("Clean").Does(() =>
	{
		DotNetCoreRestore("./Frozenskys.ConsulService.sln");
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
	{
		DotNetCoreBuild("./Frozenskys.ConsulService/Frozenskys.ConsulService.csproj", new DotNetCoreBuildSettings
			{				
				Configuration = "Release"				
			});
	});

Task("Package")
	.IsDependentOn("Build")
	.Does(() =>
	{
		var nuGetPackSettings   = new NuGetPackSettings {
                                     Id                      = "Frozenskys.ConsulService",
                                     Version                 = "0.1.0.0",
                                     Title                   = "Consul Helper for ASP.NET Core Applications",
                                     Authors                 = new[] {"Richard Cooper"},
                                     Description             = "Contains helpers to make using consul with ASP.NET core applications easier",
                                     ProjectUrl              = new Uri("https://github.com/frozenskys/consulservice/"),
                                     LicenseUrl              = new Uri("https://frozenskys.mit-license.org/"),
                                     Copyright               = "Frozenskys Software Ltd. 2017",
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
									 Dependencies            = new [] {
                                                                          new NuSpecDependency {Id="Consul", Version="0.7.2.1", TargetFramework="net452"},
                                                                       },
									 Files                   = new [] {
                                                                          new NuSpecContent {Source = "**/Frozenskys.ConsulService.dll", Target = "lib"},
                                                                       },
                                     BasePath                = buildFolder,
                                     OutputDirectory         = artifacts
                                 };

     NuGetPack(nuGetPackSettings);
	});

RunTarget(target);