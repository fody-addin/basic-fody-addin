
#tool "nuget:?package=NUnit.ConsoleRunner"

Task("test")
    .Does(() => {
        XBuild("BasicFodyAddin.sln");
        NUnit3("./Tests/bin/Debug/Tests.dll");
    });


RunTarget("test");