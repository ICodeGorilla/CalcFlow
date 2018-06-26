// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProducer.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ScriptContract
{
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis.Scripting;

    /// <summary>
    /// The Producer interface, receives instructions produces results
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of context in which the code will be executed
    /// </typeparam>
    public interface IProducer<TContext>
    {
        /// <summary>
        /// Produces result from a string commands async.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result that will be generated
        /// </typeparam>
        /// <param name="command">
        /// The commands as a string
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with a result of TResult
        /// </returns>
        Task<TResult> ProduceAsync<TResult>(string command);

        /// <summary>
        /// Produces result from a string commands async within a context.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result that will be generated
        /// </typeparam>
        /// <param name="context">
        /// The context as type of TContext.
        /// </param>
        /// <param name="command">
        /// The command as string.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with a result of TResult
        /// </returns>
        Task<TResult> ProduceAsync<TResult>(TContext context, string command);

        /// <summary>
        /// The produce context from script async.
        /// </summary>
        /// <param name="context">
        /// The context
        /// </param>
        /// <param name="script">
        /// The script
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        Task<ScriptState> ProduceContextFromScriptAsync(TContext context, Script script);

        /// <summary>
        /// Produces result from a list of commands async.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result that will be generated
        /// </typeparam>
        /// <param name="commands">
        /// The commands as an array of strings to be executed in order
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with a result of TResult
        /// </returns>
        Task<TResult> ProduceFromListAsync<TResult>(params string[] commands);

        /// <summary>
        /// Produces result from a list of commands async with context.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result that will be generated
        /// </typeparam>
        /// <param name="context">
        /// The context in which the commands will be executed.
        /// </param>
        /// <param name="commands">
        /// The commands as an array of strings to be executed in order
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with a result of TResult
        /// </returns>
        Task<TResult> ProduceFromListAsync<TResult>(TContext context, params string[] commands);

        /// <summary>
        /// Produces result from a script commands async within a context.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result that will be generated
        /// </typeparam>
        /// <param name="context">
        /// The context as type of TContext.
        /// </param>
        /// <param name="script">
        /// The script as a compiled command.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with a result of TResult when executed within TContext
        /// </returns>
        Task<TResult> ProduceFromScriptAsync<TResult>(TContext context, Script<TResult> script);

        /// <summary>
        /// Produces result from a script commands async without a context.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the result that will be generated
        /// </typeparam>
        /// <param name="script">
        /// The script as a compiled command.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with a result of TResult
        /// </returns>
        Task<TResult> ProduceFromScriptAsync<TResult>(Script<TResult> script);
    }
}