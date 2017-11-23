#addin nuget:?package=Cake.Docker

var configuration = Argument("Configuration", "Release");

var solution = "./CoreXPlatform.sln";

var dockerTag =
    HasArgument("DockerTag") ? Argument<string>("DockerTag") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number.ToString() : TravisCI.Environment.Build.BuildNumber.ToString();

var dockerFile = 
    HasArgument("DockerFile") ? Argument<string>("DockerFile") :
    AppVeyor.IsRunningOnAppVeyor ? "Dockerfile.windows" : "Dockerfile";

var artifactsDirectory = MakeAbsolute(Directory("./Artifacts"));
 
Task("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings()
        {
            Configuration = configuration
        };

        DotNetCoreBuild("./CoreXPlatform.sln", settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./test/**/*.csproj");

        foreach(var project in projects)
        {
            // string projectTestResults = $"{absoluteArtifactsDirectory}/{project.GetFilenameWithoutExtension()}.xml";

            // DotNetCoreTest(project.GetDirectory().FullPath, new DotNetCoreTestSettings()
            // {
            //     Configuration = configuration,
            //     NoBuild = true,
            //     Logger = $"trx%3bLogFileName={projectTestResults}",
            //     Framework = "netcoreapp2.0"
            // });

            // if(AppVeyor.IsRunningOnAppVeyor)
            // {
            //     AppVeyor.UploadTestResults(projectTestResults, AppVeyorTestResultsType.XUnit);
            // }
        }
    });

Task("BuildContainer")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var settings = new DockerImageBuildSettings 
        { 
            Tag = new[] { $"dockerapp:{dockerTag}" },
            File = dockerFile
        };
        
        DockerBuild(settings, ".");
    });

RunTarget("BuildContainer");