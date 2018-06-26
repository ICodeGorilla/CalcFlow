// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITree.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TreeContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// The Tree interface.
    /// </summary>
    /// <typeparam name="TContext">
    /// The context in which the tree will operate
    /// </typeparam>
    public interface ITree<TContext>
    {
        /// <summary>
        /// Execute the tree and get the context after all calculations
        /// </summary>
        /// <param name="context">
        /// The context that will be updated
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with the updated context
        /// </returns>
        Task<TContext> Run(TContext context);
    }
}