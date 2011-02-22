using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChainingAssertion
{
    [TestClass]
    public class UnitTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            // "Is" extend on all object and has three overloads.

            // This same as Assert.AreEqual(25, Math.Pow(5, 2))
            Math.Pow(5, 2).Is(25);

            // lambda predicate assertion.
            // This same as Assert.IsTrue("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
            "foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));

            // has collection assert
            // This same as CollectionAssert.AreEqual(Enumerable.Range(1,5), new[]{1, 2, 3, 4, 5})
            Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);

            // and two extension methods.
            Object obj = null;
            obj.IsNull();    // Assert.IsNull(obj)
            obj.IsNotNull(); // Assert.IsNotNull(obj)
        }

        [TestMethod]
        public void FailTest()
        {
            (100 + 300).Is(x => x < 350);
        }


        [TestMethod]
        [TestCaseSource("toaruSource")]
        public void TestTestCaseSource()
        {
            TestContext.Run((int x, int y, string z) =>
            {
                string.Concat(x, y).Is(z);
            });
        }

        public static object[] toaruSource = new[]
        {
            new object[] {1, 1, "11"},
            new object[] {5, 3, "53"},
            new object[] {9, 4, "94"}
        };




    }
}