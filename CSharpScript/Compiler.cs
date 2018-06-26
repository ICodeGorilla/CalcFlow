// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Compiler.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpScript
{
    using CSharpScript.Extension;
    using CSharpScript.Helper;

    using Microsoft.CodeAnalysis.Scripting;

    using ScriptContract;

    /// <inheritdoc />
    /// <summary>
    /// The compiler implementation that will compile string commands to be executed multiple times later.
    /// </summary>
    public class Compiler : ICompiler
    {
        /// <inheritdoc />
        /// <summary>
        /// CompileScriptWithContext without globals context
        /// </summary>
        /// <param name="command">
        /// The command as c# code.
        /// </param>
        /// <returns>
        /// Compiled <see cref="Script"/>.
        /// </returns>
        public Script<TResult> Compile<TResult>(string command)
        {
            CommandValidator.ValidateCommandIsNotNullOrEmpty<TResult, INoContext>(command);
            var script =
                Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create<TResult>(
                    command,
                    options: Options.ScriptOptions);
            return CompileScript(script);
        }

        /// <summary>
        /// The compile script with context.
        /// </summary>
        /// <param name="command">
        /// The c# command
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context in which to execute the command
        /// </typeparam>
        /// <returns>
        /// The <see cref="Script"/>.
        /// </returns>
        public Script CompileActionScriptWithContext<TContext>(string command)
        {
            CommandValidator.ValidateCommandIsNotNullOrEmpty<TContext>(command);
            var script = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(
                command,
                options: Options.ScriptOptions,
                globalsType: typeof(TContext));
            return CompileScript(script);
        }

        /// <summary>
        /// The compile method with a globals type
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result
        /// </typeparam>
        /// <typeparam name="TContext">
        /// Context in which the command will be executed
        /// </typeparam>
        /// <param name="command">
        /// The command as c# code
        /// </param>
        /// <returns>
        /// Compiled <see cref="Script"/>.
        /// </returns>
        public Script<TResult> CompileScriptWithContext<TResult, TContext>(string command)
        {
            CommandValidator.ValidateCommandIsNotNullOrEmpty<TResult, TContext>(command);
            var script = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create<TResult>(
                command,
                options: Options.ScriptOptions,
                globalsType: typeof(TContext));
            return CompileScript(script);
        }

        /// <summary>
        /// Compile the script but throw an compilation error if it failed
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result
        /// </typeparam>
        /// <param name="script">
        /// The script that will be compiled
        /// </param>
        /// <returns>
        /// Returns compiled <see cref="Script"/> with return type of TResult
        /// </returns>
        /// <exception cref="CompilationErrorException">
        /// Will be thrown if the diagnostics are not empty
        /// </exception>
        private static Script<TResult> CompileScript<TResult>(Script<TResult> script)
        {
            var diagnostics = script.Compile();
            if (diagnostics != null && diagnostics.HasCompilationErrors())
            {
                throw new CompilationErrorException($"{typeof(TResult)} - command", diagnostics);
            }

            return script;
        }
    }
}