using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;


namespace HashSlingerTests
{
    [TestFixture()]
    public class Tests
    {
        public Tests()
        { }

        [Test]
        public void Test1()
        {
            bool assertTest = false;
            Assert.IsTrue(assertTest, "Test 1 passed");
        }
    }
}
