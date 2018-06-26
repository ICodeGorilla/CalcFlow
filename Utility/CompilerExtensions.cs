using System;
using CompilerContract;

namespace RoslynCompiler
{
    public static class CompilerExtensions
    {
        public static object CompileAndCreateObject(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
        {
            return Activator.CreateInstance(instructions.ClassType, constructorParameters);
        }

        public static T CompileAndCreateObject<T>(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
        {
            return (T)compiler.CompileAndCreateObject(instructions, constructorParameters);
        }

        public static object RunProducer(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
        {
            var scriptObject = CompileAndCreateObject<IProducer>(compiler, instructions, constructorParameters);
            return scriptObject.Run();
        }

        public static void RunScript(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
        {
            var scriptObject = CompileAndCreateObject<IScript>(compiler, instructions, constructorParameters);
            scriptObject.Run();
        }
    }
}