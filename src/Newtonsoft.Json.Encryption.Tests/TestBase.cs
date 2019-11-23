using System;
using System.IO;
using System.Runtime.CompilerServices;

public static class ModuleInitializer
{
    public static void Initialize()
    {
        FixCurrentDirectory();
    }

    static void FixCurrentDirectory([CallerFilePath] string callerFilePath = "")
    {
        Environment.CurrentDirectory = Directory.GetParent(callerFilePath).FullName;
    }
}