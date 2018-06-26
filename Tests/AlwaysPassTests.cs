// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlwaysPassTests.cs" >
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using NUnit.Framework;

    /// <summary>
    /// The always pass test, if this does not run then your setup is most probably incorrect.
    /// </summary>
    [TestFixture]
    public class AlwaysPassTests
    {
        /// <summary>
        /// Should always be green
        /// </summary>
        [Test]
        public void ShouldAlwaysPassTest()
        {
            Assert.Pass("This test should always pass");
        }
    }
}