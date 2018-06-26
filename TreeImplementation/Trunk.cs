// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Trunk.cs" >
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
    /// The trunk implementation of section, mono outcome
    /// </summary>
    public class Trunk : ITrunk
    {
        /// <summary>
        /// The script of the current section, seen as action not producer
        /// </summary>
        private readonly Script script;

        /// <summary>
        /// The next section if any
        /// </summary>
        private ISection nextSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trunk"/> class.
        /// </summary>
        /// <param name="script">
        /// The script that will be executed
        /// </param>
        public Trunk(Script script)
        {
            this.script = script;
            this.nextSection = null;
        }

        /// <summary>
        /// Returns the next script
        /// </summary>
        /// <returns>
        /// The <see cref="Script"/>.
        /// </returns>
        public Script GetScript()
        {
            return this.script;
        }

        /// <summary>
        /// Get the decision script, not needed for trunk
        /// </summary>
        /// <returns>
        /// <see cref="bool"/>.
        /// </returns>
        public Script<bool> GetDecisionScript()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Is this the last section
        /// </summary>
        /// <returns>
        /// <see cref="bool"/>.
        /// </returns>
        public bool IsLeaf()
        {
            return this.nextSection == null;
        }

        /// <summary>
        /// Will always be false for a trunk
        /// </summary>
        /// <returns>
        /// <see cref="bool"/>.
        /// </returns>
        public bool IsBranch()
        {
            return false;
        }

        /// <summary>
        /// Will always be true for a trunk
        /// </summary>
        /// <returns>
        /// <see cref="bool"/>.
        /// </returns>
        public bool IsTrunk()
        {
            return true;
        }

        /// <summary>
        /// Fluent API that will add the next section
        /// </summary>
        /// <param name="nextScript">
        /// The next Script
        /// </param>
        /// <returns>
        /// <see cref="Script"/>
        /// </returns>
        public ITrunk AddSection(Script nextScript)
        {
            var trunk = new Trunk(nextScript);
            this.nextSection = trunk;
            return trunk;
        }

        /// <summary>
        /// Will always be false for a trunk
        /// </summary>
        /// <param name="createBranchScript">
        /// The create Branch Script.
        /// </param>
        /// <param name="trueTrunk">
        /// The true Trunk.
        /// </param>
        /// <param name="falseTrunk">
        /// The false Trunk.
        /// </param>
        public void AddBranch(Script<bool> createBranchScript, ITrunk trueTrunk, ITrunk falseTrunk)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Will always be false for a trunk
        /// </summary>
        /// <param name="decisionBranch">
        /// The branch Decision, irrelevant here
        /// </param>
        /// <returns>
        /// <see cref="bool"/>.
        /// </returns>
        public ISection GetNextSection(bool decisionBranch = true)
        {
            return this.nextSection;
        }

        /// <summary>
        /// Creates a fork where the calculation could go down different paths
        /// </summary>
        /// <param name="createBranchScript">
        /// The script that will determine which branch to use
        /// </param>
        /// <param name="trueTrunk">
        /// The trunk that will be executed if the branch script returns true
        /// </param>
        /// <param name="falseTrunk">
        /// The trunk that will be executed if the branch script returns false
        /// </param>
        public void AddBranch(Script<bool> createBranchScript, Trunk trueTrunk, Trunk falseTrunk)
        {
            var branch = new Branch(createBranchScript, trueTrunk, falseTrunk);
            this.nextSection = branch;
        }
    }
}