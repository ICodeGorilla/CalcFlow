// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoalSeekTests.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using System;

    using CSharpScript;

    using GoalSeekImplementation;

    using NUnit.Framework;

    using Tests.Helper;

    using TreeContract;

    using TreeImplementation;

    /// <summary>
    /// The goal seek tests.
    /// </summary>
    [TestFixture]
    // ReSharper disable once TestFileNameWarning
    public class GoalSeekTests
    {
        /// <summary>
        /// Given: I have a tree with multiple sequential sections and a target that I would like to reach
        /// When:  I execute the goal seek method
        /// Then:  I should receive the difference between the input and the required amount
        /// </summary>
        /// <param name="target">
        /// The target of the goal seek
        /// </param>
        [Test]
        public void TestGoalSeek([Values(1700, 1400, 1300)] decimal target)
        {
            // Given
            var tree = GetTree();
            var goalSeek = new GoalSeek<GoalSeekContext>(() => new GoalSeekContext(0, target), tree, ModifyContext, GetTarget, GetStartValue);

            // When
            var result = goalSeek.GetGoalSeekValue(target);
            var executionTine = goalSeek.GetExecutionTine();
            var iterations = goalSeek.GetIterations();

            // Then
            var context = new GoalSeekContext(result, target);
            tree.Run(context);
            Assert.IsNotNull(context);
            var grossUp = (long)(context.TargetDecimal - context.Tax + context.DecimalToManipulate);
            Assert.AreEqual(target, grossUp);
            Assert.IsNotNull(iterations);
            Assert.IsNotNull(executionTine);
        }

        /// <summary>
        /// Get the tree
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>ITree</cref>
        ///     </see>
        ///     .
        /// </returns>
        private static ITree<GoalSeekContext> GetTree()
        {
            var root = new Trunk(ScriptHelper.GetScript<GoalSeekContext>("Tax = (decimal)(TargetDecimal * (decimal)0.3)"));
            root.AddSection(ScriptHelper.GetScript<GoalSeekContext>("Tax = (decimal)(Tax + 1000)"))
                .AddSection(ScriptHelper.GetScript<GoalSeekContext>("TargetDecimal = TargetDecimal - Tax + DecimalToManipulate"));
            return new Tree<GoalSeekContext>(root, new Producer<GoalSeekContext>());
        }

        /// <summary>
        /// Modify the variable in the context
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="GoalSeekContext"/>.
        /// </returns>
        private static GoalSeekContext ModifyContext(GoalSeekContext context, decimal input)
        {
            context.DecimalToManipulate = input;
            return context;
        }

        /// <summary>
        /// Get the target
        /// </summary>
        /// <param name="context">
        /// The context from which to retrieve the target
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        private static decimal GetTarget(GoalSeekContext context)
        {
            return context.TargetDecimal;
        }

        /// <summary>
        /// The get start value.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        private static decimal GetStartValue(GoalSeekContext context)
        {
            return context.Tax;
        }

        /// <summary>
        /// The test context. DO NOT MAKE PRIVATE IT WILL STOP EXECUTION
        /// </summary>
        public class GoalSeekContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GoalSeekContext"/> class. 
            /// </summary>
            /// <param name="decimalToManipulate">
            /// The decimal that we will change
            /// </param>
            /// <param name="targetDecimal">
            /// The decimal we would like to get to a certain value
            /// </param>
            public GoalSeekContext(decimal decimalToManipulate, decimal targetDecimal)
            {
                this.DecimalToManipulate = decimalToManipulate;
                this.TargetDecimal = targetDecimal;
                this.Tax = 0;
            }

            /// <summary>
            /// Gets or sets the decimal to manipulate.
            /// </summary>
            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public decimal DecimalToManipulate { get; set; }

            /// <summary>
            /// Gets or sets the tax.
            /// </summary>
            public decimal Tax { get; set; }

            /// <summary>
            /// Gets or sets the target decimal.
            /// </summary>
            public decimal TargetDecimal { get; set; }
        }
    }
}
