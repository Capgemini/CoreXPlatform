#addin nuget:?package=Cake.Docker

var configuration = Argument("Configuration", "Release");

var solution = "./CoreXPlatform.sln";

var buildNumber =
    HasArgument("BuildNumber") ? Argument<string>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number.ToString() : TravisCI.Environment.Build.BuildNumber.ToString();

var dockerFile = 
    HasArgument("DockerFile") ? Argument<string>("DockerFile") :
    AppVeyor.IsRunningOnAppVeyor ? "Dockerfile.windows" : "Dockerfile";

var artifactsDirectory = MakeAbsolute(Directory("./Artifacts"));

var commitTag =
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Repository.Tag.Name :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.Tag : null;
 
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
            string projectTestResults = $"{artifactsDirectory}/{project.GetFilenameWithoutExtension()}-{buildNumber}.xml";

            DotNetCoreTool(
                projectPath: project.FullPath, 
                command: "xunit", 
                arguments: $"-configuration {configuration} -diagnostics -stoponfail --fx-version 2.0.0 -xml {projectTestResults}"
            );

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

        if (!string.IsNullOrWhiteSpace(commitTag))
        {
            Information($"Build Tagged with tag {commitTag}, starting publish of docker image to registry.");

            settings.Tag = new[] { $"dockerapp:{commitTag}" };

            DockerBuild(settings, ".");
        }
        else
        {
            Information("No tag found, ignoring publish.");
        }
    });

RunTarget("BuildContainer");