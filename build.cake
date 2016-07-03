
#tool "nuget:?package=NUnit.ConsoleRunner"

Task("test")
    .Does(() => {
        XBuild("BasicFodyAddin.sln");
        NUnit3("./BasicFodyAddin.Tests/bin/Debug/Tests.dll");
    });


RunTarget("test");