// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandValidator.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpScript.Helper
{
    using System.Collections.Generic;

    using CSharpScript.Exception;

    using ScriptContract;

    /// <summary>
    /// Validates if we have commands that are not null or empty
    /// </summary>
    public static class CommandValidator
    {
        /// <summary>
        /// The validate command is not null or empty.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <typeparam name="TResult">
        /// Type of result used when throwing error
        /// </typeparam>
        /// <typeparam name="TContext">
        /// Type of context used when throwing error
        /// </typeparam>
        /// <exception cref="NoCodeToCompileException{TResult,TContext}">
        /// Will be thrown if null or empty
        /// </exception>
        public static void ValidateCommandIsNotNullOrEmpty<TResult, TContext>(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new NoCodeToCompileException<TResult, TContext>();
            }
        }

        /// <summary>
        /// The validate command list is not null or empty.
        /// </summary>
        /// <param name="commandList">
        /// The command list.
        /// </param>
        /// <typeparam name="TResult">
        /// Type of result used when throwing error
        /// </typeparam>
        /// <typeparam name="TContext">
        /// Type of context used when throwing error
        /// </typeparam>
        /// <exception cref="NoCodeToCompileException{T, T}">
        /// Will be thrown if null or empty
        /// </exception>
        public static void ValidateCommandIsNotNullOrEmpty<TResult, TContext>(IList<string> commandList)
        {
            if (commandList == null || commandList.Count == 0)
            {
                throw new NoCodeToCompileException<TResult, TContext>();
            }
        }

        /// <summary>
        /// The validate command is not null or empty.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <typeparam name="TContext">
        /// Type of context used when throwing error
        /// </typeparam>
        /// <exception cref="NoCodeToCompileException{T, T}">
        /// Will be thrown if null or empty
        /// </exception>
        internal static void ValidateCommandIsNotNullOrEmpty<TContext>(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new NoCodeToCompileException<TContext, INoContext>();
            }
        }
    }
}