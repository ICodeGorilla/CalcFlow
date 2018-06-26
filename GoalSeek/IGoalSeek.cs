// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoalSeek.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GoalSeekContract
{
    /// <summary>
    /// Gross up application to determine the values required to reach a target
    /// </summary>
    public interface IGoalSeek
    {
        /// <summary>
        /// Get the value of the gross up component 
        /// </summary>
        /// <param name="targetValue">
        /// The target Value.
        /// </param>
        /// <returns>
        /// The difference between the existing item and what is required to achieve the target value
        /// </returns>
        decimal GetGoalSeekValue(decimal targetValue);
    }
}
