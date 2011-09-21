using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChainingAssertion
{
    [TestClass]
    public class UnitTest
    {
        public TestContext TestContext { get; set; }

        // samples

        public class Hoge
        {
            public string MyProperty { get; set; }
        }

        [TestMethod]
        public void IsTest()
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
        }

        [TestMethod]
        public void CollectionTest()
        {
            // if you want to use CollectionAssert Methods then use Linq to Objects and Is

            var array = new[] { 1, 3, 7, 8 };
            array.Count().Is(4);
            array.Contains(8).Is(true);
            array.All(i => i < 5).Is(false);
            array.Any().Is(true);
            new int[] { }.Any().Is(false);   // IsEmpty
            array.OrderBy(x => x).Is(array); // IsOrdered
        }

        [TestMethod]
        public void OthersTest()
        {
            // Null Assertions
            Object obj = null;
            obj.IsNull();             // Assert.IsNull(obj)
            new Object().IsNotNull(); // Assert.IsNotNull(obj)

            // Not Assertion
            "foobar".IsNot("fooooooo"); // Assert.AreNotEqual
            new[] { "a", "z", "x" }.IsNot("a", "x", "z"); /// CollectionAssert.AreNotEqual

            // ReferenceEqual Assertion
            var tuple = new { foo = "foo" };
            tuple.IsSameReferenceAs(tuple); // Assert.AreSame
            tuple.IsNotSameReferenceAs(new { foo = "foo" }); // Assert.AreNotSame

            // Type Assertion
            "foobar".IsInstanceOf<string>(); // Assert.IsInstanceOfType
            (999).IsNotInstanceOf<double>(); // Assert.IsNotInstanceOfType
        }

        [TestMethod]
        public void AdvancedCollectionTest()
        {
            var lower = new[] { "a", "b", "c" };
            var upper = new[] { "A", "B", "C" };

            // Comparer CollectionAssert, use IEqualityComparer<T> or Func<T,T,bool> delegate
            lower.Is(upper, StringComparer.InvariantCultureIgnoreCase);
            lower.Is(upper, (x, y) => x.ToUpper() == y.ToUpper());

            // or you can use Linq to Objects - SequenceEqual
            lower.SequenceEqual(upper, StringComparer.InvariantCultureIgnoreCase).Is(true);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            // Exception Test(alternative of ExpectedExceptionAttribute)
            // Throws does not allow derived type
            // Catch allows derived type
            AssertEx.Throws<ArgumentNullException>(() => "foo".StartsWith(null));
            AssertEx.Catch<Exception>(() => "foo".StartsWith(null));

            // return value is occured exception
            var ex = AssertEx.Throws<InvalidOperationException>(() =>
            {
                throw new InvalidOperationException("foobar operation");
            });
            ex.Message.Is(s => s.Contains("foobar")); // additional exception assertion

            // must not throw any exceptions
            AssertEx.DoesNotThrow(() =>
            {
                // code

            });
        }

        public class Person
        {
            public int Age { get; set; }
            public string FamilyName { get; set; }
            public string GivenName { get; set; }
        }

        [TestMethod]
        public void DumpTest()
        {
            var count = new List<int>() { 1, 2, 3 };
            var person = new Person { Age = 50, FamilyName = "Yamamoto", GivenName = "Tasuke" };
            try
            {
                person.Is(p => p.Age < count.Count && p.FamilyName == "Yamada" && p.GivenName == "Tarou");
            }
            catch (Exception ex)
            {
                ex.Message.Contains("Age = 50, FamilyName = Yamamoto, GivenName = Tasuke").Is(true);
                return;
            }
            Assert.Fail();
        }

        // exceptions

        [TestMethod]
        public void Throws()
        {
            try
            {
                AssertEx.Throws<Exception>(() => "foo".StartsWith(null));
            }
            catch (AssertFailedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Catch()
        {
            try
            {
                AssertEx.Catch<Exception>(() => "foo".StartsWith(null));
            }
            catch (AssertFailedException)
            {
                Assert.Fail();
            }
            return;
        }

        [TestMethod]
        public void Throws2()
        {
            try
            {
                AssertEx.Throws<Exception>(() => { });
            }
            catch (AssertFailedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Catch2()
        {
            try
            {
                AssertEx.Catch<Exception>(() => { });
            }
            catch (AssertFailedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Exception()
        {
            var ex = AssertEx.Throws<ArgumentNullException>(() =>
            {
                throw new ArgumentNullException("nullnull");
            });
        }
    }
}