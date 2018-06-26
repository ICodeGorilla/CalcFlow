// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeTests.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using CSharpScript;

    using Microsoft.CodeAnalysis.Scripting;

    using NUnit.Framework;

    using ScriptContract;

    using Tests.Helper;

    using TreeContract;

    using TreeImplementation;

    /// <summary>
    /// The decision tree tests.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1404:CodeAnalysisSuppressionMustHaveJustification",
        Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]

    // ReSharper disable once TestFileNameWarning
    public class TreeTests
    {
        /// <summary>
        /// The producer instance used throughout
        /// </summary>
        private static readonly IProducer<TreeTestContext> Producer = new Producer<TreeTestContext>();

        /// <summary>
        /// Given I have a tree that has only a simple calculation and a valid context
        /// When I run the tree
        /// Then I should receive the expected result
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task RootTest()
        {
            // Given
            ISection root = new Trunk(ScriptHelper.GetScript<TreeTestContext>("Z = X + Y"));
            ITree<TreeTestContext> tree = new Tree<TreeTestContext>(root, Producer);
            var context = new TreeTestContext(1, 3);

            // When
            await tree.Run(context);

            // Then
            Assert.AreEqual(4, context.Z);
        }

        /// <summary>
        /// Given I have a tree that multiple sequential calculations and a valid context
        /// When I run the tree
        /// Then I should receive the expected result
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task MultipleSequentialSegmentsTest()
        {
            // Given
            var root = new Trunk(CreateScript("Z = X + Y"));
            root.AddSection(CreateScript("Z = Z * 10")).AddSection(CreateScript("Y = Y * Y")).AddSection(CreateScript("X = -123"));
            ITree<TreeTestContext> tree = new Tree<TreeTestContext>(root, Producer);
            var context = new TreeTestContext(1, 3);

            // When
            await tree.Run(context);

            // Then
            Assert.AreEqual(-123, context.X);
            Assert.AreEqual(9, context.Y);
            Assert.AreEqual(40, context.Z);
        }

        /// <summary>
        /// Given I have a tree that has two branches that will return and the context requires the false branch to be executed
        /// When I run the tree
        /// Then I should receive the expected result from the false branch
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task BranchWithFalseFlowTest()
        {
            // Given
            ITree<TreeTestContext> tree = new Tree<TreeTestContext>(GetBranchTest(), Producer);
            var context = new TreeTestContext(1, 3);

            // When
            await tree.Run(context);

            // Then
            Assert.AreEqual(99, context.Z);
        }

        /// <summary>
        /// Given I have a tree that has two branches that will return and the context requires the true branch to be executed
        /// When I run the tree
        /// Then I should receive the expected result from the false branch
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task BranchWithTrueFlowTest()
        {
            // Given
            ITree<TreeTestContext> tree = new Tree<TreeTestContext>(GetBranchTest(), Producer);
            var context = new TreeTestContext(3, 1);

            // When
            await tree.Run(context);

            // Then
            Assert.AreEqual(150, context.Y);
        }

        /// <summary>
        /// Basic branch flow, reused to check true and false flows
        /// </summary>
        /// <returns>
        /// The <see cref="Trunk"/>.
        /// </returns>
        private static Trunk GetBranchTest()
        {
            var root = new Trunk(CreateScript("Z = X + Y"));
            root.AddBranch(CreateBranchScript("return X > Y;"), new Trunk(CreateScript("Y = 150")), new Trunk(CreateScript("Z = 99")));
            return root;
        }

        /// <summary>
        /// Create an action script, just a wrapper to make the tests cleaner
        /// </summary>
        /// <param name="command">
        /// The command as text
        /// </param>
        /// <returns>
        /// The action <see cref="Script"/>.
        /// </returns>
        private static Script CreateScript(string command)
        {
            return ScriptHelper.GetScript<TreeTestContext>(command);
        }

        /// <summary>
        /// Wrapper to make the code more readable
        /// </summary>
        /// <param name="command">
        /// The command as string
        /// </param>
        /// <returns>
        /// The <see cref="Script"/> with a bool response
        /// </returns>
        private static Script<bool> CreateBranchScript(string command)
        {
            return ScriptHelper.GetBranchScript<TreeTestContext>(command);
        }

        /// <summary>
        /// The test context. DO NOT MAKE PRIVATE IT WILL STOP EXECUTION
        /// </summary>
        public class TreeTestContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TreeTestContext"/> class. 
            /// </summary>
            /// <param name="x">
            /// The x decimal value
            /// </param>
            /// <param name="y">
            /// The y decimal value
            /// </param>
            public TreeTestContext(decimal x, decimal y)
            {
                this.X = x;
                this.Y = y;
                this.Z = 0;
            }

            /// <summary>
            /// Gets or sets the x.
            /// </summary>
            public decimal X { get; set; }

            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            public decimal Y { get; set; }

            /// <summary>
            /// Gets or sets the z.
            /// </summary>
            public decimal Z { get; set; }
        }
    }
}