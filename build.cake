var taskNames = new {
	Default = "Default",
	Clean = "Clean",
	Restore = "Restore",
	Build = "Build",
	Pack = "Pack"
};

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var artifactsDirectory = Directory("./artifacts");
var revision = EnvironmentVariable("APPVEYOR_BUILD_NUMBER") ?? "1";
revision = Convert.ToInt32(revision, 10).ToString("0000");

Task(taskNames.Default)
	.IsDependentOn(taskNames.Pack)
	.Does(() =>
{
});

Task("Clean")
.Does(() =>
{
    CleanDirectory(artifactsDirectory);
});

Task(taskNames.Restore)
.Does(() => 
{
    DotNetCoreRestore();
});

Task(taskNames.Build)
.IsDependentOn(taskNames.Clean)
.IsDependentOn(taskNames.Restore)
.Does(() =>
{
    DotNetCoreBuild("./src/**/project.json", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        VersionSuffix = revision
    });
});

Task(taskNames.Pack)
.IsDependentOn(taskNames.Build)
.Does(() =>
{
    var projects = GetFiles("./src/**/project.json");
    foreach (var project in projects)
    {
        DotNetCorePack(project.FullPath, new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = artifactsDirectory,
            VersionSuffix = revision
        });
    }
});

RunTarget(target);