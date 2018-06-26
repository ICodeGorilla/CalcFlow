using System.Reflection;

namespace CompilerContract
{
    public interface ICompiler
    {
        Assembly Compile(string code, params string[] assemblyLocations);
    }
}