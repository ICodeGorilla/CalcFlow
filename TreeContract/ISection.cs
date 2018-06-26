// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISection.cs"  >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TreeContract
{
    using Microsoft.CodeAnalysis.Scripting;

    /// <summary>
    /// The Section interface.
    /// </summary>
    public interface ISection
    {
        /// <summary>
        /// Returns script
        /// </summary>
        /// <returns>
        /// The <see cref="Script"/>.
        /// </returns>
        Script GetScript();

        /// <summary>
        /// Is this the last section in the tree
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsLeaf();

        /// <summary>
        /// Is this a branch
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsBranch();

        /// <summary>
        /// Is this a trunk
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsTrunk();

        /// <summary>
        /// Get the next section if any
        /// </summary>
        /// <param name="decisionBranch">
        /// The decision Branch bool, will send back different section if branch type
        /// </param>
        /// <returns>
        /// The <see cref="ISection"/>, could be different depending on the branch logic
        /// </returns>
        ISection GetNextSection(bool decisionBranch = true);
    }
}