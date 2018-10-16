// (c) 2018 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
#addin "Cake.Compression"
#addin "SharpZipLib"
#addin "cake.docfx"
#addin "altcover.api"
#tool "NUnit.ConsoleRunner"
#tool "ReportGenerator"
#tool "docfx.console"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

var msbuildConfig = new MSBuildSettings {
    Verbosity = Verbosity.Minimal,
    Configuration = configuration,
    Restore = true,
    MaxCpuCount = 0,  // Auto build parallel mode
    WarningsAsError = Argument("warnaserror", false)
};

Task("Build-API")
    .Does(() =>
{
    MSBuild("src/Connector.sln", msbuildConfig);
});

Task("Build-Examples")
    .IsDependentOn("Build-API")
    .Does(() =>
{
    MSBuild("examples/Simple/Simple.sln", msbuildConfig);
    MSBuild("examples/Mixed/Mixed.sln", msbuildConfig);
    MSBuild("examples/Objects/Objects.sln", msbuildConfig);
});

Task("Run-UnitTests")
    .IsDependentOn("Build-API")
    .Does(() =>
{
    string testProjectDir = "src/Connector.UnitTests";
    string testProject = $"{testProjectDir}/Connector.UnitTests.csproj";

    // NUnit3 to test libraries with .NET Framework / Mono
    var nunitSettings = new NUnit3Settings {
        EnvironmentVariables = new Dictionary<string, string> {
            { GetLoadLibraryEnvVar(), GetNativeLibraryPath() }
        }
    };
    NUnit3(
        $"{testProjectDir}/bin/{configuration}/net45/*.UnitTests.dll",
        nunitSettings);

    // .NET Core test library
    var netcoreSettings = new DotNetCoreTestSettings {
        EnvironmentVariables = new Dictionary<string, string> {
            { GetLoadLibraryEnvVar(), GetNativeLibraryPath() }
        },
        NoBuild = true,
        Framework = "netcoreapp2.1"
    };
    DotNetCoreTest(testProject, netcoreSettings);
});

Task("Run-Linter-Gendarme")
    .IsDependentOn("Build-API")
    .Does(() =>
{
    var mono_tools = DownloadFile("https://github.com/pleonex/mono-tools/releases/download/v4.2.2/mono-tools-v4.2.2.zip");
    ZipUncompress(mono_tools, "tools/mono_tools");
    var gendarme = "tools/mono_tools/bin/gendarme";
    if (!IsRunningOnWindows()) {
        if (StartProcess("chmod", $"+x {gendarme}") != 0) {
            Error("Cannot change gendarme permissions");
        }
    }

    RunGendarme(
        gendarme,
        $"src/Connector/bin/{configuration}/net35/librtiddsconnector_dotnet.dll",
        "src/Connector/Gendarme.ignore");
});

Task("Run-AltCover")
    .IsDependentOn("Build-API")
    .Does(() =>
{
    // Configure the tests to run with code coverate
    TestWithAltCover(
        "src/Connector.UnitTests",
        "librtiddsconnector_dotnet.UnitTests.dll",
        "coverage.xml");

    // Create the report
    var reportTypes = new[] {
        ReportGeneratorReportType.Html,
        ReportGeneratorReportType.XmlSummary };
    ReportGenerator(
        "coverage.xml",
        "coverage_report",
        new ReportGeneratorSettings { ReportTypes = reportTypes });

    // Get final result
    var xml = System.Xml.Linq.XDocument.Load("coverage_report/Summary.xml");
    var coverage = xml.Root.Element("Summary").Element("Linecoverage").Value;
    if (coverage == "100%") {
        Information("Full coverage!");
    } else {
        Error($"Missing coverage: {coverage}");
    }
});

Task("Test-Quality")
    .Description("Run quality assurance tasks")
    .IsDependentOn("Run-Linter-Gendarme");
    .IsDependentOn("Run-AltCover");

Task("Fix-DocFx")
    .Description("Workaround for issue #3389: missing dependency")
    .Does(() =>
{
    // Workaround for
    // https://github.com/dotnet/docfx/issues/3389
    NuGetInstall("SQLitePCLRaw.core", new NuGetInstallSettings {
        ExcludeVersion  = true,
        OutputDirectory = "./tools"
    });

    CopyFileToDirectory(
        "tools/SQLitePCLRaw.core/lib/net45/SQLitePCLRaw.core.dll",
        GetDirectories("tools/docfx.console.*").Single().Combine("tools"));
});

Task("Generate-DocWeb")
    .Description("Generate a static web with the documentation")
    .IsDependentOn("Build-API")
    .IsDependentOn("Fix-DocFx")
    .Does(() =>
{
    DocFxMetadata("docs/docfx.json");
    DocFxBuild("docs/docfx.json");
});

Task("Generate-DocPdf")
    .Description("Generate a PDF with the documentation")
    .IsDependentOn("Build-API")
    .IsDependentOn("Fix-DocFx")
    .Does(() =>
{
    DocFxMetadata("docs/docfx.json");
    DocFxPdf("docs/docfx.json");
});

Task("Update-DocRepo")
    .Description("Commit and push the latest documentation to the repository")
    .IsDependentOn("Generate-DocWeb")
    .Does(() =>
{
   int retcode;

    // Clone or pull
    var repo_doc = Directory("docs/repo");
    if (!DirectoryExists(repo_doc)) {
        retcode = StartProcess(
            "git",
            $"clone git@github.com:rticommunity/rticonnextdds-connector-cs {repo_doc} -b gh-pages");
        if (retcode != 0) {
            throw new Exception("Cannot clone repository");
        }
    } else {
        retcode = StartProcess("git", new ProcessSettings {
            Arguments = "pull",
            WorkingDirectory = repo_doc
        });
        if (retcode != 0) {
            throw new Exception("Cannot pull repository");
        }
    }

    // Copy the content of the web
    CopyDirectory("docs/_site", repo_doc);

    // Commit and push
    retcode = StartProcess("git", new ProcessSettings {
        Arguments = "commit -a -m ':books: Update doc from cake'",
        WorkingDirectory = repo_doc
    });
    if (retcode != 0) {
        throw new Exception("Cannot commit doc repo");
    }

    retcode = StartProcess("git", new ProcessSettings {
        Arguments = "push origin gh-pages",
        WorkingDirectory = repo_doc
    });
    if (retcode != 0) {
        throw new Exception("Cannot push doc repo");
    }
});

Task("Default")
    .IsDependentOn("Build-API")
    .IsDependentOn("Build-Examples")
    .IsDependentOn("Run-UnitTests")
    .IsDependentOn("Test-Quality");

Task("Travis")
    .IsDependentOn("Build-API")
    .IsDependentOn("Build-Examples")
    .IsDependentOn("Run-UnitTests")
    .IsDependentOn("Test-Quality")
    .IsDependentOn("Generate-DocWeb");  // Validate documentation but don't update

RunTarget(target);


public string GetLoadLibraryEnvVar()
{
    if (IsRunningOnWindows()) {
        return "PATH";
    } else if (IsRunningOnMacOSX()) {
        return "DYLD_LIBRARY_PATH";
    } else if (IsRunningOnUnix()) {
        return "LD_LIBRARY_PATH";
    }

    throw new Exception("Unsupported platform");
}

public string GetNativeLibraryPath(bool x86 = false)
{
    string arch;
    if (IsRunningOnWindows()) {
        arch = x86 ? "i86Win32VS2010" : "x64Win64VS2013";
    } else if (IsRunningOnMacOSX()) {
        if (x86) {
            throw new Exception("32-bits not supported on MacOSX");
        }

        arch = "x64Darwin16clang8.0";
    } else if (IsRunningOnUnix()) {
        arch = x86 ? "i86Linux3.xgcc4.6.3" : "x64Linux2.6gcc4.4.5";
    } else {
        throw new Exception("Unsupported platform");
    }

    return MakeAbsolute(Directory($"rticonnextdds-connector/lib/{arch}")).FullPath;
}

public bool IsRunningOnMacOSX()
{
    return Environment.OSVersion.Platform == PlatformID.MacOSX;
}

public void RunGendarme(string gendarme, string assembly, string ignore)
{
    var retcode = StartProcess(gendarme, $"--ignore {ignore} {assembly}");
    if (retcode != 0) {
        throw new Exception($"Gendarme found errors on {assembly}");
    }
}

public void TestWithAltCover(string projectPath, string assembly, string outputXml)
{
    string inputDir = $"{projectPath}/bin/{configuration}/net45";
    string outputDir = $"{inputDir}/__Instrumented";
    if (DirectoryExists(outputDir)) {
        DeleteDirectory(
            outputDir,
            new DeleteDirectorySettings { Recursive = true });
    }

    var altcoverArgs = new AltCover.PrepareArgs {
        InputDirectory = inputDir,
        OutputDirectory = outputDir,
        AssemblyFilter = new[] { "nunit.framework" },
        XmlReport = outputXml,
        OpenCover = true
    };
    Prepare(altcoverArgs);

    var nunitSettings = new NUnit3Settings {
        EnvironmentVariables = new Dictionary<string, string> {
            { GetLoadLibraryEnvVar(), GetNativeLibraryPath() }
        },
        NoResults = true
    };
    NUnit3($"{outputDir}/{assembly}", nunitSettings);
}
