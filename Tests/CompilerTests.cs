// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompilerTests.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CSharpScript;
    using CSharpScript.Exception;

    using Microsoft.CodeAnalysis.Scripting;

    using NUnit.Framework;

    using ScriptContract;

    /// <summary>
    /// The compiler tests.
    /// </summary>
    [TestFixture]
    // ReSharper disable once TestFileNameWarning
    public class CompilerTests
    {
        /// <summary>
        /// The compiled scripts to be used in tests later
        /// </summary>
        private readonly Dictionary<string, Script<List<int>>> compiledScripts;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerTests"/> class.
        /// </summary>
        public CompilerTests()
        {
            this.compiledScripts = new Dictionary<string, Script<List<int>>> { { "Test1", GetPreCompiledScript() } };
        }

        /// <summary>
        /// Given: I have valid c# commands
        /// When:  I compile it
        /// Then:  I should be able to call it multiple times without recompiling and retrieve the globals
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task CompileAndRunActionCommandTest()
        {
            // Given
            var producer = new Producer<TestContext>();
            var command = @"var x = new List<int>(){1,2,3,4,5}; ActionResult = x.Take(3).ToList();";
            var compiler = new Compiler();
            var compiledScript = compiler.CompileActionScriptWithContext<TestContext>(command);
            var context = new TestContext();

            // When 
            await producer.ProduceContextFromScriptAsync(context, compiledScript);

            // Then
            Assert.AreEqual(3, context.ActionResult.Count);
        }

        /// <summary>
        /// Given: I have valid c# commands
        /// When:  I compile it
        /// Then:  I should be able to call it multiple times without recompiling
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task CompileAndRunCommandTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var command = @"var x = new List<int>(){1,2,3,4,5}; var y = x.Take(3).ToList(); return y;";
            var compiler = new Compiler();
            var compiledScript = compiler.Compile<IList<int>>(command);
            var firstRunTask = producer.ProduceFromScriptAsync(compiledScript);
            var secondRunTask = producer.ProduceFromScriptAsync(compiledScript);

            // When 
            await Task.WhenAll(firstRunTask, secondRunTask);

            // Then
            Assert.AreEqual(3, firstRunTask.Result.Count);
            Assert.AreEqual(3, secondRunTask.Result.Count);
        }

        /// <summary>
        /// Given: I have a c# commands that will return a subset of an existing list depending on its context
        /// When:  I compile it
        /// Then:  I should be able to call it multiple times without recompiling and receive the correct response for each context
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task CompileAndRunWithParametersCommandTest()
        {
            // Given
            var producer = new Producer<TestContext>();
            var command = @"var x = new List<int>(){1,2,3,4,5}; var y = x.Take(Count).ToList(); return y;";
            var compiler = new Compiler();
            var script = compiler.CompileScriptWithContext<List<int>, TestContext>(command);
            var firstRunTask = producer.ProduceFromScriptAsync(new TestContext { Count = 1 }, script);
            var secondRunTask = producer.ProduceFromScriptAsync(new TestContext { Count = 2 }, script);
            var thirdRunTask = producer.ProduceFromScriptAsync(new TestContext { Count = 3 }, script);
            var fourthRunTask = producer.ProduceFromScriptAsync(new TestContext { Count = 4 }, script);

            // When 
            await Task.WhenAll(firstRunTask, secondRunTask, thirdRunTask, fourthRunTask);

            // Then
            Assert.AreEqual(1, firstRunTask.Result.Count);
            Assert.AreEqual(2, secondRunTask.Result.Count);
            Assert.AreEqual(3, thirdRunTask.Result.Count);
            Assert.AreEqual(4, fourthRunTask.Result.Count);
        }

        /// <summary>
        /// Given: I have a invalid c# commands
        /// When:  I compile it
        /// Then:  I should receive an 
        /// </summary>
        [Test]
        public void CompilingCommandFailsTest()
        {
            // Given
            var command = @"var x = new List<int>(){1,2,3,4 var y = x.Take(3).ToList(); return y;";
            var compiler = new Compiler();

            // When, Then
            Assert.Throws<CompilationErrorException>(() => compiler.Compile<IList<int>>(command));
        }

        /// <summary>
        /// Given: I have an empty c# commands
        /// When:  I compile it
        /// Then:  I should receive an 
        /// </summary>
        [Test]
        public void CompilingEmptyCommandFailsTest()
        {
            // Given
            var command = string.Empty;
            var compiler = new Compiler();

            // When, Then
            Assert.Throws<NoCodeToCompileException<IList<int>, INoContext>>(
                () => compiler.Compile<IList<int>>(command));
        }

        /// <summary>
        /// Given: I have an invalid c# command that will return a subset of an existing list
        /// When:  I execute it
        /// Then:  I should receive an compilation exception
        /// </summary>
        [Test]
        public void RunCommandFailsTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var command = @"var x = new List<int>(){1,2,3,4,5 var y = x.Take(3).ToList(); return y;";

            // When, Then
            Assert.ThrowsAsync<CompilationErrorException>(async () => await producer.ProduceAsync<IList>(command));
        }

        /// <summary>
        /// Given: I have a list of invalid c# commands that will return a subset of an existing list
        /// When:  I execute it
        /// Then:  I should receive an CompilationErrorException
        /// </summary>
        [Test]
        public void RunCommandListFailsTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var commands = new List<string>
                               {
                                   "var x = new List<int>(){1,2,3,4,5",
                                   "var y = x.Take(3).ToList();",
                                   "return y;"
                               };

            // When, Then
            Assert.ThrowsAsync<CompilationErrorException>(
                async () => await producer.ProduceFromListAsync<IList>(commands.ToArray()));
        }

        /// <summary>
        /// Given: I have a list of valid c# commands that will return a subset of an existing list
        /// When:  I execute it
        /// Then:  I should receive the correct number of items in the list
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task RunCommandListTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var commands = new List<string>
                               {
                                   "var x = new List<int>(){1,2,3,4,5};",
                                   "var y = x.Take(3).ToList();",
                                   "return y;"
                               };

            // When 
            var result = await producer.ProduceFromListAsync<IList>(commands.ToArray());

            // Then
            Assert.AreEqual(3, result.Count);
        }

        /// <summary>
        /// Given: I have a list of invalid c# commands that will return a subset of an existing list and a context object that will be used during execution
        /// When:  I execute it
        /// Then:  I should receive an NoCodeToCompileException
        /// </summary>
        [Test]
        public void RunCommandListWithParametersFailsTest()
        {
            // Given
            var context = new TestContext { Count = 3 };
            var producer = new Producer<TestContext>();
            var commands = new List<string>
                               {
                                   "var x = new List<int>(){1,2,3,",
                                   "var y = x.Take(Count).ToList();",
                                   "return y;"
                               };

            // When, Then
            Assert.ThrowsAsync<CompilationErrorException>(
                async () => await producer.ProduceFromListAsync<IList>(context, commands.ToArray()));
        }

        /// <summary>
        /// Given: I have a list of valid c# commands that will return a subset of an existing list and a context object that will be used during execution
        /// When:  I execute it
        /// Then:  I should receive the correct number of items in the list
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task RunCommandListWithParametersTest()
        {
            // Given
            var context = new TestContext { Count = 3 };
            var producer = new Producer<TestContext>();
            var commands = new List<string>
                               {
                                   "var x = new List<int>(){1,2,3,4,5};",
                                   "var y = x.Take(Count).ToList();",
                                   "return y;"
                               };

            // When 
            var result = await producer.ProduceFromListAsync<IList>(context, commands.ToArray());

            // Then
            Assert.AreEqual(3, result.Count);
        }

        /// <summary>
        /// Given: I have a c# command that will return a subset of an existing list
        /// When:  I execute it
        /// Then:  I should receive the correct number of items in the list
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task RunCommandTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var command = @"var x = new List<int>(){1,2,3,4,5}; var y = x.Take(3).ToList(); return y;";

            // When 
            var result = await producer.ProduceAsync<IList>(command);

            // Then
            Assert.AreEqual(3, result.Count);
        }

        /// <summary>
        /// Given: I have an invalid c# command that will return a subset of an existing list with a context object that will be used during execution
        /// When:  I execute it
        /// Then:  I should receive a CompilationErrorException
        /// </summary>
        [Test]
        public void RunCommandWithParametersFailsTest()
        {
            // Given
            var context = new TestContext { Count = 3 };
            var producer = new Producer<TestContext>();
            var command = @"var x = new List<int>(){1,2,3,4, var y = x.Take(Count).ToList(); return y;";

            // When, Then
            Assert.ThrowsAsync<CompilationErrorException>(
                async () => await producer.ProduceAsync<IList>(context, command));
        }

        /// <summary>
        /// Given: I have a c# command that will return a subset of an existing list with a context object that will be used during execution
        /// When:  I execute it
        /// Then:  I should receive an exception
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [Test]
        public async Task RunCommandWithParametersTest()
        {
            // Given
            var context = new TestContext { Count = 3 };
            var producer = new Producer<TestContext>();
            var command = @"var x = new List<int>(){1,2,3,4,5}; var y = x.Take(Count).ToList(); return y;";

            // When 
            var result = await producer.ProduceAsync<IList>(context, command);

            // Then
            Assert.AreEqual(3, result.Count);
        }

        /// <summary>
        /// Given: I have an empty c# command that will return a subset of an existing list
        /// When:  I execute it
        /// Then:  I should receive an NoCodeToCompileException exception
        /// </summary>
        [Test]
        public void RunEmptyCommandFailsTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var command = string.Empty;

            // When, Then
            Assert.ThrowsAsync<NoCodeToCompileException<IList, INoContext>>(
                async () => await producer.ProduceAsync<IList>(command));
        }

        /// <summary>
        /// Given: I have an empty command
        /// When:  I execute it
        /// Then:  I should receive an NoCodeToCompileException exception
        /// </summary>
        [Test]
        public void RunEmptyCommandListFailsTest()
        {
            // Given
            var producer = new Producer<INoContext>();
            var commands = new List<string>();

            // When, Then
            Assert.ThrowsAsync<NoCodeToCompileException<IList, INoContext>>(
                async () => await producer.ProduceFromListAsync<IList>(commands.ToArray()));
        }

        /// <summary>
        /// Given: I have an empty list of invalid c# commands and a context object that will be used during execution
        /// When:  I execute it
        /// Then:  I should receive an NoCodeToCompileException
        /// </summary>
        [Test]
        public void RunEmptyCommandListWithParametersFailsTest()
        {
            // Given
            var context = new TestContext { Count = 3 };
            var producer = new Producer<TestContext>();
            var commands = new List<string>();

            // When, Then
            Assert.ThrowsAsync<NoCodeToCompileException<IList, TestContext>>(
                async () => await producer.ProduceFromListAsync<IList>(context, commands.ToArray()));
        }

        /// <summary>
        /// Given: I have an empty c# command that will return a subset of an existing list with a context object that will be used during execution
        /// When:  I execute it
        /// Then:  I should receive a CompilationErrorException
        /// </summary>
        [Test]
        public void RunEmptyCommandWithParametersFailsTest()
        {
            // Given
            var context = new TestContext { Count = 3 };
            var producer = new Producer<TestContext>();
            var command = string.Empty;

            // When, Then
            Assert.ThrowsAsync<NoCodeToCompileException<IList, TestContext>>(
                async () => await producer.ProduceAsync<IList>(context, command));
        }

        /// <summary>
        /// Given: I have a pre-compiled script that will return a subset of an existing list depending on its context
        /// When:  I execute it
        /// Then:  I should be able to call it multiple times without recompiling and receive the correct response for each context
        /// </summary>
        /// <param name="take">
        /// Number of items to take from the list
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>
        /// </returns>
        [TestCase(1, 7)]
        [TestCase(2, 6)]
        [TestCase(3, 5)]
        [TestCase(4, 4)]
        public async Task RunPreCompiledScriptWithParametersCommandTest(int take, int expectedResult)
        {
            // Given
            var context = new TestContext { Count = take };
            var producer = new Producer<TestContext>();
            var script = this.compiledScripts.First();

            // When 
            var result = await producer.ProduceFromScriptAsync(context, script.Value);

            // Then
            Assert.AreEqual(expectedResult, result.Last());
        }

        /// <summary>
        /// The get pre-compiled script that will be used in the tests.
        /// </summary>
        /// <returns>
        /// The <see cref="Script"/>. that will be reused
        /// </returns>
        private static Script<List<int>> GetPreCompiledScript()
        {
            var command = @"var x = new List<int>(){7,6,5,4,3,2,1}; var y = x.Take(Count).ToList(); return y;";
            var compiler = new Compiler();
            return compiler.CompileScriptWithContext<List<int>, TestContext>(command);
        }

        /// <summary>
        /// The test context.
        /// </summary>
        /// DO NOT MAKE PRIVATE IT WILL BREAK EXECUTION
        public class TestContext
        {
            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            public List<int> ActionResult { get; set; }

            /// <summary>
            /// Gets or sets the count.
            /// </summary>
            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public int Count { get; set; }
        }
    }
}