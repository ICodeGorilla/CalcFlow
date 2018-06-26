// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Branch.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TreeImplementation
{
    using Microsoft.CodeAnalysis.Scripting;

    using TreeContract;

    /// <summary>
    /// The branch implementation of section, binary outcome
    /// </summary>
    public class Branch : ISection
    {
        private readonly Script<bool> decisionScript;
        private readonly ISection trueBranch;
        private readonly ISection falseBranch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Branch"/> class. 
        /// </summary>
        /// <param name="decisionScript">
        /// The bool valued script that will be used to determine which branch to use
        /// </param>
        /// <param name="trueBranch">
        /// The true Branch.
        /// </param>
        /// <param name="falseBranch">
        /// The false Branch.
        /// </param>
        public Branch(Script<bool> decisionScript, ISection trueBranch, ISection falseBranch)
        {
            this.decisionScript = decisionScript;
            this.trueBranch = trueBranch;
            this.falseBranch = falseBranch;
        }

        /// <summary>
        /// Returns the next section determined by the decisionScript
        /// </summary>
        /// <param name="branchDecision">
        /// The branch Decision.
        /// </param>
        /// <returns>
        /// The <see cref="Script"/>.
        /// </returns>
        public ISection GetNextSection(bool branchDecision)
        {
            return branchDecision ? this.trueBranch : this.falseBranch;
        }

        /// <summary>
        /// Returns the next script
        /// </summary>
        /// <returns>
        /// The <see cref="Script"/>.
        /// </returns>
        public Script GetScript()
        {
            return this.decisionScript;
        }

        /// <summary>
        /// Returns if this is a leaf, will always be false for a branch
        /// </summary>
        /// <returns>
        /// Returns if this is a leaf or not
        /// </returns>
        public bool IsLeaf()
        {
            return false;
        }

        /// <summary>
        /// Returns if this is a branch, will always be true for a branch
        /// </summary>
        /// <returns>
        /// Returns if this is a leaf or not
        /// </returns>
        public bool IsBranch()
        {
            return true;
        }

        /// <summary>
        /// Returns if this is a leaf, will always be false for a branch
        /// </summary>
        /// <returns>
        /// Returns if this is a leaf or not
        /// </returns>
        public bool IsTrunk()
        {
            return false;
        }
    }
}
