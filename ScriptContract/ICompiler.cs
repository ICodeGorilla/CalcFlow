// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompiler.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ScriptContract
{
    using Microsoft.CodeAnalysis.Scripting;

    /// <summary>
    /// The Compiler interface.
    /// </summary>
    public interface ICompiler
    {
        /// <summary>
        /// The compiler method with no global context.
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned when executing the compiled script
        /// </typeparam>
        /// <param name="command">
        /// C# code as text
        /// </param>
        /// <returns>
        /// <see cref="Script"/> with a result of TResult
        /// </returns>
        Script<TResult> Compile<TResult>(string command);

        /// <summary>
        /// Compile script with that will be executed context.
        /// </summary>
        /// <typeparam name="TContext">
        /// Type of context
        /// </typeparam>
        /// <param name="command">
        /// C# code as text
        /// </param>
        /// <returns>
        /// <see cref="Script"/> with a result of TResult
        /// </returns>
        Script CompileActionScriptWithContext<TContext>(string command);

        /// <summary>
        /// Compile script with that will be executed context.
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned when executing the compiled script
        /// </typeparam>
        /// <typeparam name="TContext">
        /// Type of context
        /// </typeparam>
        /// <param name="command">
        /// C# code as text
        /// </param>
        /// <returns>
        /// <see cref="Script"/> with a result of TResult
        /// </returns>
        Script<TResult> CompileScriptWithContext<TResult, TContext>(string command);
    }
}