// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpScript
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.Scripting;

    /// <summary>
    /// The standard options options that will be used when compiling/running a script.
    /// </summary>
    public static class Options
    {
        /// <summary>
        /// Gets the script options, contains default imports and references
        /// </summary>
        public static ScriptOptions ScriptOptions
        {
            get
            {
                var scriptOptions = ScriptOptions.Default;
                scriptOptions = scriptOptions.AddReferences(typeof(object).Assembly, typeof(Enumerable).Assembly);
                scriptOptions = scriptOptions.AddImports(
                    new List<string> { "System", "System.Linq", "System.Collections.Generic" });
                return scriptOptions;
            }
        }
    }
}