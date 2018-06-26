using System;
using System.Collections.Generic;
using CompilerContract;

namespace RoslynCompiler
{
    public class CompilerInstructions : ICompilerInstructions
    {
        public List<string> AssemblyLocations { get; } = new List<string>();
        public string ClassName { get; set; }
        public string Code { get; set; }
        string[] ICompilerInstructions.AssemblyLocations => AssemblyLocations.ToArray();
        public Type ClassType { get; set; }
    }
}