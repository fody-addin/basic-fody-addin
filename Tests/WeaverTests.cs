using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class WeaverTests
{
    private Assembly assembly;
    private string newAssemblyPath;
    private string assemblyPath;

    [OneTimeSetUp]
    public void Setup()
    {
		var dir = new DirectoryInfo("./").FullName;
		Console.WriteLine(dir);
		assemblyPath = Path.Combine(dir, "Tests/bin/Debug/AssemblyToProcess.dll");

        newAssemblyPath = assemblyPath.Replace(".dll", "2.dll");

        File.Copy(assemblyPath, newAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(newAssemblyPath);

        var weavingTask = new ModuleWeaver
        {
            ModuleDefinition = moduleDefinition
        };

        weavingTask.Execute();
        moduleDefinition.Write(newAssemblyPath);

        assembly = Assembly.LoadFile(newAssemblyPath);
    }

    [Test]
    public void ValidateHelloWorldIsInjected()
    {
        var type = assembly.GetType("Hello");
        var instance = (dynamic)Activator.CreateInstance(type);

        Assert.AreEqual("Hello World", instance.World());
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assemblyPath,newAssemblyPath);
    }
#endif
}