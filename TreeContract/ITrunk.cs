namespace TreeContract
{
    using Microsoft.CodeAnalysis.Scripting;

    /// <summary>
    /// The trunk interface - simple calculation
    /// </summary>
    public interface ITrunk : ISection
    {
        /// <summary>
        /// Fluent call to add sections
        /// </summary>
        /// <param name="nextScript">
        /// The script.
        /// </param>
        /// <returns>
        /// The last section
        /// </returns>
        ITrunk AddSection(Script nextScript);

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
        void AddBranch(Script<bool> createBranchScript, ITrunk trueTrunk, ITrunk falseTrunk);
    }
}