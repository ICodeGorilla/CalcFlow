using System;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynCompiler
{
    public class RoslynCompilationException : Exception
    {
        public EmitResult Result { get; private set; }

        public RoslynCompilationException(string message, EmitResult result) : base(message)
        {
            Result = result;
        }
    }
}