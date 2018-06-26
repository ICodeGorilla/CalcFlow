// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticsExtensions.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpScript.Extension
{
    using System.Collections.Immutable;
    using System.Linq;

    using Microsoft.CodeAnalysis;

    /// <summary>
    /// The diagnostics extensions.
    /// </summary>
    public static class DiagnosticsExtensions
    {
        /// <summary>
        /// Check if there are any errors after compilation
        /// </summary>
        /// <param name="diagnostics">
        /// The diagnostics.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasCompilationErrors(this ImmutableArray<Diagnostic> diagnostics)
        {
            return diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error);
        }
    }
}