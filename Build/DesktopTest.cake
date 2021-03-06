#tool "nuget:?package=OpenCover"
#tool coveralls.io
#addin Cake.Coveralls
using System.Linq;

// --------------------------------------------------------------------------------
// Desktop Build and Test
// --------------------------------------------------------------------------------

var NET_CORE_TEST_PROJ_DIR = "../Tests/SafeApp.Tests.Core/";
var coreTestProject = File($"{NET_CORE_TEST_PROJ_DIR}SafeApp.Tests.Core.csproj");
var coreTestBin = Directory("${NET_CORE_TEST_PROJ_DIR}bin/Release");
var codeCoverageFilePath = $"{NET_CORE_TEST_PROJ_DIR}CodeCoverResult.xml";
var Desktop_TESTS_RESULT_PATH = $"{NET_CORE_TEST_PROJ_DIR}TestResults/DesktopTestResult.xml";
var Desktop_test_result_directory = $"{NET_CORE_TEST_PROJ_DIR}TestResults";
var coveralls_token = EnvironmentVariable("coveralls_access_token");

Task("Build-Desktop-Project")
  .IsDependentOn("Restore-NuGet")
  .Does(() => {
    var dotnetBuildArgument = string.Empty;
    var osFamily = (int)Context.Environment.Platform.Family;
    if(osFamily == 1)
      dotnetBuildArgument = @"-p:RuntimeIdentifiers=win10-x64";
    else if(osFamily == 2)
      dotnetBuildArgument = @"-p:RuntimeIdentifiers=linux-x64";
    else if (osFamily == 3)
      dotnetBuildArgument = @"-p:RuntimeIdentifiers=osx-x64";
    
	var buildSettings = new DotNetCoreMSBuildSettings()
	{
	  ArgumentCustomization = args => args.Append(dotnetBuildArgument)
	};
	buildSettings.SetConfiguration(configuration);
    DotNetCoreMSBuild(coreTestProject, buildSettings);
  });

Task("Run-Desktop-Tests")
  .IsDependentOn("Build-Desktop-Project")
  .Does(() => {
      DotNetCoreTest(
        coreTestProject.Path.FullPath,
        new DotNetCoreTestSettings()
        {
		  NoBuild = true,
		  NoRestore = true,
          Configuration = configuration,
          ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=DesktopTestResult.xml\"")
        });
  });

Task("Run-Desktop-Tests-AppVeyor")
  .IsDependentOn("Build-Desktop-Project")
  .Does(() => {
    OpenCover(tool => {
      tool.DotNetCoreTest(
          coreTestProject,
          new DotNetCoreTestSettings()
          {
			NoBuild = true,
			NoRestore = true,
            Configuration = configuration,
            ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=DesktopTestResult.xml\"")
          });
    },
    new FilePath(codeCoverageFilePath),
    new OpenCoverSettings()
    {
        SkipAutoProps = true,
        Register = "user",
        OldStyle = true
    }
    .WithFilter("+[*]*")
    .WithFilter("-[SafeApp.Tests*]*")
    .WithFilter("-[NUnit3.*]*"));
  })
  .Finally(() =>
  {
    var resultFile = string.Empty; 
    if (FileExists(Desktop_TESTS_RESULT_PATH))
    {
      resultFile = File(Desktop_TESTS_RESULT_PATH);
    }
    else
    {
      resultFile = GetFiles(Desktop_test_result_directory + "/DesktopTestResult*.xml").LastOrDefault().FullPath;
    }

    if (!string.IsNullOrEmpty(resultFile))
    {
      Information($"Test result file found: {resultFile}");
      Desktop_TESTS_RESULT_PATH = resultFile;

      if(AppVeyor.IsRunningOnAppVeyor)
      {
        AppVeyor.UploadTestResults(resultFile, AppVeyorTestResultsType.MSTest);
      }
    }
    else
    {
      throw new Exception("Test result file not found.");
    }
    
    // todo : enable after adding the new API
    // if(EnvironmentVariable("is_not_pr") == "true")
    // {
    //   CoverallsIo(codeCoverageFilePath, new CoverallsIoSettings()
    //   {
    //     RepoToken = coveralls_token
    //   });
    // }
  });
