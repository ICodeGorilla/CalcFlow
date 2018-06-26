// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptHelper.cs"  >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests.Helper
{
    using CSharpScript;

    using Microsoft.CodeAnalysis.Scripting;

    /// <summary>
    /// This Script Helper will give you basic compiled scripts that use decimals X and Y and give a result Z
    /// </summary>
    public static class ScriptHelper
    {
        /// <summary>
        /// The shared compiler.
        /// </summary>
        private static readonly Compiler Compiler = new Compiler();

        /// <summary>
        /// Get script
        /// </summary>
        /// <param name="command">
        /// The c# command.
        /// </param>
        /// <typeparam name="TContext">
        /// Context used to execute
        /// </typeparam>
        /// <returns>
        /// The <see cref="Script"/>. with a return type of decimal
        /// </returns>
        public static Script GetScript<TContext>(string command)
        {
            return Compiler.CompileActionScriptWithContext<TContext>(command);
        }

        /// <summary>
        /// Get branch script
        /// </summary>
        /// <param name="command">
        /// The command that will return a bool
        /// </param>
        /// <typeparam name="TContext">
        /// Context used to execute
        /// </typeparam>
        /// <returns>
        /// <see cref="Script"/>
        /// </returns>
        public static Script<bool> GetBranchScript<TContext>(string command)
        {
            return Compiler.CompileScriptWithContext<bool, TContext>(command);
        }
    }
}