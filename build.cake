#addin nuget:?package=Cake.Docker

var configuration = Argument("Configuration", "Release"); // only build release configurations

var solution = "./CoreXPlatform.sln";

// take the build number as an argument or from the CI system
var buildNumber =
    HasArgument("BuildNumber") ? Argument<string>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number.ToString() : TravisCI.Environment.Build.BuildNumber.ToString();

// take the docker file to build the image as an argument when running the script locally, otherwise Linux for Trvais or Windows for AppVeyor 
var dockerFile = 
    HasArgument("DockerFile") ? Argument<string>("DockerFile") :
    AppVeyor.IsRunningOnAppVeyor ? "Dockerfile.windows" : "Dockerfile";

var artifactsDirectory = MakeAbsolute(Directory("./Artifacts"));

// if the check-in on the builf system is a tagged release
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

        // build the release configuration of the solution 
        DotNetCoreBuild("./CoreXPlatform.sln", settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./test/**/*.csproj");

        // test each project using xunit and publish the results to the artifacts folder
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
            Tag = new[] { $"dockerapp:1.0.{buildNumber}" },
            File = dockerFile
        };
        // build the docker image based on the type of envioronment we are running in. just use the build number as a tag
        DockerBuild(settings, ".");

        // if this build has been tagged, then publish it to a registry
        if (!string.IsNullOrWhiteSpace(commitTag))
        {
            Information($"Build Tagged with tag {commitTag}, starting publish of docker image to registry.");

            settings.Tag = new[] { $"dockerapp:{commitTag}" };

            DockerBuild(settings, "."); // this is a bit weird, i need to rebuild because the docker plugin for cake doesnt support tagging?

            // to do: publish
        }
        else
        {
            Information("No tag found, ignoring publish.");
        }
    });

RunTarget("BuildContainer");