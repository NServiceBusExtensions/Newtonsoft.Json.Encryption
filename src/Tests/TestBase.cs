using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

public abstract class TestBase
{
    ITestOutputHelper output;

    static TestBase()
    {
        FixCurrentDirectory();
    }

    static void FixCurrentDirectory([CallerFilePath] string callerFilePath = "")
    {
        Environment.CurrentDirectory = Directory.GetParent(callerFilePath).FullName;
    }

    protected TestBase(ITestOutputHelper output)
    {
        this.output = output;
    }
}