// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoCodeToCompileException.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpScript.Exception
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// Will be thrown when we have an empty or null command
    /// </summary>
    /// <typeparam name="TResult">
    /// Expected result
    /// </typeparam>
    /// <typeparam name="TContext">
    /// Expected context 
    /// </typeparam>
    public class NoCodeToCompileException<TResult, TContext> : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoCodeToCompileException{TResult,TContext}"/> class.
        /// </summary>
        public NoCodeToCompileException()
            : base($"{typeof(TResult)} - {typeof(TContext)}")
        {
        }
    }
}