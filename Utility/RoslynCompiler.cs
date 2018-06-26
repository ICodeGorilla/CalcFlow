using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CompilerContract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynCompiler
{
    public class RoslynCompiler : ICompiler
    {
        /// Gets the compilation options.
        public CSharpCompilationOptions Options { get; } = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            true,
            optimizationLevel: OptimizationLevel.Release,
            generalDiagnosticOption: ReportDiagnostic.Error
        );

        /// Compiles the specified code the sepcified assembly locations.
        public Assembly Compile(string code, params string[] assemblyLocations)
        {
            var references = assemblyLocations.Select(l => MetadataReference.CreateFromFile(l));

            var compilation = CSharpCompilation.Create(
                "_" + Guid.NewGuid().ToString("D"),
                references: references,
                syntaxTrees: new[] { CSharpSyntaxTree.ParseText(code) },
                options: Options
            );

            using (var ms = new MemoryStream())
            {
                var compilationResult = compilation.Emit(ms);

                if (compilationResult.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    return Assembly.Load(ms.ToArray());
                }

                throw new RoslynCompilationException("Assembly could not be created.", compilationResult);
            }
        }
    }
}