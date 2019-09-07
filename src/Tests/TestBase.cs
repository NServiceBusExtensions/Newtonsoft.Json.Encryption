using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

public class TestBase:
    XunitApprovalBase
{
    public TestBase(ITestOutputHelper output) :
        base(output)
    {
    }

    static TestBase()
    {
        FixCurrentDirectory();
    }

    static void FixCurrentDirectory([CallerFilePath] string callerFilePath = "")
    {
        Environment.CurrentDirectory = Directory.GetParent(callerFilePath).FullName;
    }
}