// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Producer.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpScript
{
    using System.Linq;
    using System.Threading.Tasks;

    using CSharpScript.Helper;

    using Microsoft.CodeAnalysis.Scripting;

    using ScriptContract;

    /// <summary>
    /// The producer class that will execute a code from either code as string or script
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the global context, can be none if so use INoContext
    /// </typeparam>
    public class Producer<TContext> : IProducer<TContext>
    {
        /// <summary>
        /// Simplest execution of C# code as string async.
        /// </summary>
        /// <param name="command">
        /// The c# command
        /// </param>
        /// <typeparam name="TResult">
        /// The result that will be returned
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>
        /// Task with TResult response
        /// </returns>
        public Task<TResult> ProduceAsync<TResult>(string command)
        {
            return this.ProduceAsync<TResult>(default(TContext), command);
        }

        /// <summary>
        /// Produces result with simple string as command with a context
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned
        /// </typeparam>
        /// <param name="context">
        /// The context in which to execute the command
        /// </param>
        /// <param name="command">
        /// The command as c# script
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>
        /// Will return TResult
        /// </returns>
        /// <exception cref="CSharpScript.Exception">
        /// Will return exception if no code, or compilation exception if 
        /// </exception>
        public async Task<TResult> ProduceAsync<TResult>(TContext context, string command)
        {
            CommandValidator.ValidateCommandIsNotNullOrEmpty<TResult, TContext>(command);
            return await Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.EvaluateAsync<TResult>(
                       command,
                       Options.ScriptOptions,
                       context);
        }

        /// <summary>
        /// Produces result from Script with Context
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="script">
        /// The action script
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with execution state
        /// </returns>
        public Task<ScriptState> ProduceContextFromScriptAsync(TContext context, Script script)
        {
            return script.WithOptions(Options.ScriptOptions).RunAsync(context);
        }

        /// <summary>
        /// The produce result from list of commands async.
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned
        /// </typeparam>
        /// <param name="commands">
        /// The commands as a string array
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> of type T.
        /// </returns>
        public async Task<TResult> ProduceFromListAsync<TResult>(params string[] commands)
        {
            return await this.ProduceFromListAsync<TResult>(default(TContext), commands);
        }

        /// <summary>
        /// The produce from list async.
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned
        /// </typeparam>
        /// <param name="context">
        /// The context as TGlobal
        /// </param>
        /// <param name="commands">
        /// Array of C# commands to be executed in order within the context
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with type of TResult.
        /// </returns>
        /// <exception cref="CSharpScript.Exception">
        /// Will throw compilation exception if the code in the string is not correct
        /// </exception>
        public async Task<TResult> ProduceFromListAsync<TResult>(TContext context, params string[] commands)
        {
            CommandValidator.ValidateCommandIsNotNullOrEmpty<TResult, TContext>(commands);
            var state = await Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.RunAsync(
                            commands.First(),
                            Options.ScriptOptions,
                            context);
            foreach (var command in commands.Skip(1))
            {
                state = await state.ContinueWithAsync(command);
            }

            return (TResult)state.ReturnValue;
        }

        /// <summary>
        /// Produces result from Script with Context
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned
        /// </typeparam>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="script">
        /// The script with a response type of TResult
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with result of TResult
        /// </returns>
        public async Task<TResult> ProduceFromScriptAsync<TResult>(TContext context, Script<TResult> script)
        {
            return (await script.WithOptions(Options.ScriptOptions).RunAsync(context)).ReturnValue;
        }

        /// <summary>
        /// Produces result from script with no context
        /// </summary>
        /// <typeparam name="TResult">
        /// The result that will be returned
        /// </typeparam>
        /// <param name="script">
        /// The script of type TResult
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>
        /// Will return result
        /// </returns>
        public Task<TResult> ProduceFromScriptAsync<TResult>(Script<TResult> script)
        {
            return this.ProduceFromScriptAsync(default(TContext), script);
        }
    }
}