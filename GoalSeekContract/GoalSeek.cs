// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoalSeek.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GoalSeekImplementation
{
    using System;

    using Accord.Math.Optimization;

    using GoalSeekContract;

    using TreeContract;

    /// <inheritdoc />
    /// <summary>
    /// GoalSeek Implementation
    /// </summary>
    /// <typeparam name="TContext">
    /// Context in which the goal seek will be executed
    /// </typeparam>
    public class GoalSeek<TContext> : IGoalSeek
    {
        /// <summary>
        /// The context function.
        /// </summary>
        private readonly Func<TContext> contextFunc;

        /// <summary>
        /// The tree.
        /// </summary>
        private readonly ITree<TContext> tree;

        /// <summary>
        /// The variable to manipulate function.
        /// </summary>
        private readonly Func<TContext, decimal, TContext> variableToManipulateFunc;

        /// <summary>
        /// The target function.
        /// </summary>
        private readonly Func<TContext, decimal> targetFunc;

        /// <summary>
        /// The start value function.
        /// </summary>
        private readonly Func<TContext, decimal> startValueFunc;

        /// <summary>
        /// The target value.
        /// </summary>
        private decimal target;

        /// <summary>
        /// The time span to calculate
        /// </summary>
        private TimeSpan timeSpan;

        /// <summary>
        /// The iterations used to calculate
        /// </summary>
        private int iterations;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalSeek{TContext}"/> class. 
        /// Main constructor
        /// </summary>
        /// <param name="contextFunc">
        /// Function to create a fresh context as we will manipulate it with the tree
        /// </param>
        /// <param name="tree">
        /// The calculation as a tree
        /// </param>
        /// <param name="variableToManipulateFunc">
        /// Function to set the variable for goal seek
        /// </param>
        /// <param name="targetFunc">
        /// Function to retrieve the variable that we would like match
        /// </param>
        /// <param name="startValueFunc">
        /// Function that will help us narrow the start value
        /// </param>
        public GoalSeek(Func<TContext> contextFunc, ITree<TContext> tree, Func<TContext, decimal, TContext> variableToManipulateFunc, Func<TContext, decimal> targetFunc, Func<TContext, decimal> startValueFunc)
        {
            this.contextFunc = contextFunc;
            this.tree = tree;
            this.variableToManipulateFunc = variableToManipulateFunc;
            this.targetFunc = targetFunc;
            this.startValueFunc = startValueFunc;
        }

        /// <summary>
        /// Get the minimum value that we can add to hit the target
        /// </summary>
        /// <param name="targetValue">
        /// The target we would like to hit
        /// </param>
        /// <returns>
        /// The difference between the initial value and what we need to add/remove to hit the target
        /// </returns>
        public decimal GetGoalSeekValue(decimal targetValue)
        {
            this.iterations = 0;
            var startTime = DateTime.Now;
            this.target = targetValue;
            var startValue = this.GetStartValue();
            var result = (decimal)BrentSearch.FindRoot(this.TestTree, (double)startValue, (double)targetValue, 1);
            this.timeSpan = DateTime.Now - startTime;

            return result;
        }

        /// <summary>
        /// Test method to see how efficient the brent search is
        /// </summary>
        /// <returns>Number of iterations</returns>
        public int GetIterations()
        {
            return this.iterations;
        }

        /// <summary>
        /// Test method to see how efficient the brent search is
        /// </summary>
        /// <returns>Time span</returns>
        public TimeSpan GetExecutionTine()
        {
            return this.timeSpan;
        }

        /// <summary>
        /// Get start value.
        /// </summary>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        private decimal GetStartValue()
        {
            var context = this.contextFunc();
            this.tree.Run(context);
            var startValue = this.startValueFunc(context);
            return startValue;
        }

        /// <summary>
        /// The function to check if the value is correct
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        private double TestTree(double input)
        {
            this.iterations++;
            var context = this.contextFunc();
            this.variableToManipulateFunc(context, (decimal)input);
            this.tree.Run(context);
            var currentTarget = this.targetFunc(context);
            var testResult = (double)Math.Floor(this.target - currentTarget);
            return testResult;
        }
    }
}
