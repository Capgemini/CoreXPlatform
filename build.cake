#addin nuget:?package=Cake.Docker

// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");

// Configuration - The build configuration (Debug/Release) to use.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if an Environment variable exists, use that.
var configuration = 
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") : "Release";

// The build number to use in the version number of the built NuGet packages.
// There are multiple ways this value can be passed, this is a common pattern.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if running on AppVeyor, get it's build number.
// 3. Otherwise if running on Travis CI, get it's build number.
// 4. Otherwise if an Environment variable exists, use that.
// 5. Otherwise default the build number to 0.
var buildNumber =
    HasArgument("BuildNumber") ? Argument<string>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number.ToString() : TravisCI.Environment.Build.BuildNumber.ToString();

var dockerFile = AppVeyor.IsRunningOnAppVeyor ? "Dockerfile.windows" : "Dockerfile";

// A directory path to an Artifacts directory.
var artifactsDirectory = Directory("./Artifacts");
var absoluteArtifactsDirectory = MakeAbsolute(Directory(artifactsDirectory));
 
// Deletes the contents of the Artifacts folder if it should contain anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
    });
 
// Find all csproj projects and build them using the build configuration specified as an argument.
 Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings()
        {
            Configuration = configuration
        };

        DotNetCoreBuild("./CoreXPlatform.sln", settings);
    });

// Look under a 'Tests' folder and run dotnet test against all of those projects.
// Then drop the XML test results file in the Artifacts folder at the root.
Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./test/**/*.csproj");

        foreach(var project in projects)
        {
            string projectTestResults = $"{absoluteArtifactsDirectory}/{project.GetFilenameWithoutExtension()}.xml";

            DotNetCoreTest(project.GetDirectory().FullPath, new DotNetCoreTestSettings()
            {
                Configuration = configuration,
                NoBuild = true,
                Logger = $"trx%3bLogFileName={projectTestResults}",
                Framework = "netcoreapp2.0"
            });

            if(AppVeyor.IsRunningOnAppVeyor)
            {
                AppVeyor.UploadTestResults(projectTestResults, AppVeyorTestResultsType.XUnit);
            }
        }
    });

Task("BuildContainer")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var settings = new DockerImageBuildSettings 
        { 
            Tag = new[] { $"dockerapp:{buildNumber}" },
            File = dockerFile
        };
        
        DockerBuild(settings, ".");
    });

// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Pack.
Task("Default")
    .IsDependentOn("BuildContainer");
 
// Executes the task specified in the target argument.
RunTarget(target);