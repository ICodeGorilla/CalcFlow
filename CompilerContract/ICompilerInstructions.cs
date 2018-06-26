using System;

namespace CompilerContract
{
    public interface ICompilerInstructions
    {
        string[] AssemblyLocations { get; }
        string Code { get; }
        string ClassName { get; }
        Type ClassType { get; set; }
    }
}