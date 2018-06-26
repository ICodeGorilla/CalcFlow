// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tree.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TreeImplementation
{
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis.Scripting;

    using ScriptContract;

    using TreeContract;

    /// <summary>
    /// The tree implementation, handles chained calculations
    /// </summary>
    /// <typeparam name="TContext">
    /// The context that will be updated by the tree
    /// </typeparam>
    public class Tree<TContext> : ITree<TContext>
    {
        /// <summary>
        /// The producer instance that will handle
        /// </summary>
        private readonly IProducer<TContext> producer;

        /// <summary>
        /// The root section
        /// </summary>
        private readonly ISection root;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree{TContext}"/> class.
        /// </summary>
        /// <param name="root">
        /// The root section
        /// </param>
        /// <param name="producer">
        /// The producer instance
        /// </param>
        public Tree(ISection root, IProducer<TContext> producer)
        {
            this.root = root;
            this.producer = producer;
        }

        /// <summary>
        /// Run from the root
        /// </summary>
        /// <param name="context">
        /// The context of the calculation
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with the updated context
        /// </returns>
        public Task<TContext> Run(TContext context)
        {
            return this.Run(context, this.root);
        }

        /// <summary>
        /// The recursive run call that will run until we hit the leaf section
        /// </summary>
        /// <param name="context">
        /// The context which will be used and updated in the calculation
        /// </param>
        /// <param name="section">
        /// The section that is currently being executed
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> with the context updated
        /// </returns>
        private async Task<TContext> Run(TContext context, ISection section)
        {
            if (section.IsTrunk())
            {
                return await this.RunTrunk(context, section);
            }

            return await this.RunBranch(context, section);
        }

        private async Task<TContext> RunBranch(TContext context, ISection section)
        {
            var branchScript = section.GetScript() as Script<bool>;
            var branchDecision = await this.producer.ProduceFromScriptAsync(context, branchScript);
            var se = section.GetNextSection(branchDecision);
            return await this.Run(context, se);
        }

        private async Task<TContext> RunTrunk(TContext context, ISection section)
        {
            await this.producer.ProduceContextFromScriptAsync(context, section.GetScript());
            if (section.IsLeaf())
            {
                return context;
            }

            var nextSection = section.GetNextSection();
            return await this.Run(context, nextSection);
        }
    }
}