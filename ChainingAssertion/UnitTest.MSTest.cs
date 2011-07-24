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
            var tuple = Tuple.Create("foo");
            tuple.IsSameReferenceAs(tuple); // Assert.AreSame
            tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.AreNotSame

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

        // dynamic

        public class PrivateMock
        {
            private string privateField = "homu";

            private string PrivateProperty
            {
                get { return privateField + privateField; }
                set { privateField = value; }
            }

            private string PrivateMethod(int count)
            {
                return string.Join("", Enumerable.Repeat(privateField, count));
            }

            private string NullableMethod(IEnumerable<int> xs)
            {
                return "enumerable";
            }

            private string NullableMethod(List<int> xs)
            {
                return "list";
            }

            private char this[int index]
            {
                get { return privateField[index]; }
                set { privateField = new string(value, index); }
            }

            private string this[double index]
            {
                get { return index.ToString(); }
                set { privateField = value + index.ToString(); }
            }
        }

        [TestMethod]
        public void DynamicTest()
        {
            var d = new PrivateMock().AsDynamic();

            (d.privateField as string).Is("homu");
            (d.PrivateProperty as string).Is("homuhomu");
            (d.PrivateMethod(3) as string).Is("homuhomuhomu");

            d.privateField = "mogu";
            (d.privateField as string).Is("mogu");

            d.PrivateProperty = "mami";
            (d.privateField as string).Is("mami");

            ((char)d[2]).Is('m');
            d[3] = 'A';
            (d.privateField as string).Is("AAA");

            ((string)d[100.101]).Is("100.101");
            d[72] = "Chihaya";
            (d.privateField as string).Is("Chihaya72");

            var e1 = AssertEx.Throws<ArgumentException>(() => { var x = d["hoge"]; });
            e1.Message.Is(s => s.Contains("indexer not found"));

            var e2 = AssertEx.Throws<ArgumentException>(() => { d["hoge"] = "a"; });
            e2.Message.Is(s => s.Contains("indexer not found"));
        }

        [TestMethod]
        public void DynamicNullableTest()
        {
            var d = new PrivateMock().AsDynamic();

            (d.NullableMethod((IEnumerable<int>)null) as string).Is("enumerable");
            (d.NullableMethod((List<int>)null) as string).Is("list");

            (d.NullableMethod(Enumerable.Range(1, 10)) as string).Is("enumerable");
            (d.NullableMethod(new List<int>().AsEnumerable()) as string).Is("enumerable");
            (d.NullableMethod(new List<int>()) as string).Is("list");
        }

        public class GenericPrivateMock
        {
            private string PrivateGeneric<T1, T2>(T1 t1a, T2 t2a, T1 t1b)
            {
                return "a";
            }

            private string PrivateGeneric<T1, T2, T3>(T1 t1a, T2 t2a, T1 t1b)
            {
                return "b";
            }

            private string PrivateGeneric<T1, T2>(T1 t1a, T2 t2a, int i)
            {
                return "c";
            }

            private string PrivateGeneric<T1, T2>(T1 t1a, T2 t2a, int i, T2 t2b)
            {
                return "d";
            }

            private string PrivateGeneric(string t1a, string t2a, string t1b)
            {
                return "e";
            }

            private string PrivateGeneric<T1, T2, T3>(T3 t3a, T2 t2, T1 t1, T3 t3b)
            {
                return "f";
            }

            private string PrivateGeneric<T>()
            {
                return "g";
            }

            private string PrivateGeneric()
            {
                return "h";
            }

            private Type ReturnType<T>(T t1, T t2)
            {
                return typeof(T);
            }

            private Type ReturnType<T>(IEnumerable<T> t1, T t2)
            {
                return typeof(T);
            }

            private string DictGen<T1, T2, T3>(IDictionary<T1, IDictionary<T2, T3>> dict, T3 xxx)
            {
                return "dict";
            }
        }

        [TestMethod]
        public void GenericPrivateTest()
        {
            var d = new GenericPrivateMock().AsDynamic();

            (d.PrivateGeneric("", 0, "") as string).Is("a");
            (d.PrivateGeneric<string, int>("", 0, "") as string).Is("a");
            (d.PrivateGeneric<int, string, long>(0, "", 0) as string).Is("b");
            (d.PrivateGeneric(0.0, "", 0) as string).Is("c");
            (d.PrivateGeneric<double, string>(0.0, "", 0) as string).Is("c");
            (d.PrivateGeneric(0.0, "", 0, "") as string).Is("d");
            (d.PrivateGeneric<double, string>(0.0, "", 0, "") as string).Is("d");
            (d.PrivateGeneric("", "", "") as string).Is("e");
            (d.PrivateGeneric(0.0, "", 0, 0.0) as string).Is("f");
            (d.PrivateGeneric<int, string, double>(0.0, "", 0, 0.0) as string).Is("f");
            (d.PrivateGeneric<int>() as string).Is("g");
            (d.PrivateGeneric() as string).Is("h");
            (d.ReturnType(0, 0) as Type).Is(typeof(int));

            (d.PrivateGeneric(0, "", 0) as string).Is("c");
            (d.PrivateGeneric<int, string>(0, "", 0) as string).Is("c");
            (d.PrivateGeneric(0, 0, 0) as string).Is("c");
            (d.ReturnType<IEnumerable<int>>(Enumerable.Range(1, 10), new List<int>()) as Type).Is(typeof(IEnumerable<int>));
        }

        [TestMethod]
        public void GenericPrivateExceptionTest()
        {
            var d = new GenericPrivateMock().AsDynamic();

            var e1 = AssertEx.Throws<ArgumentException>(() => d.HogeHoge());
            e1.Message.Is(s => s.Contains("not found") && s.Contains("HogeHoge"));

            var e2 = AssertEx.Throws<ArgumentException>(() => d.PrivateGeneric(1));
            e2.Message.Is(s => s.Contains("not match arguments") && s.Contains("PrivateGeneric"));

            var e3 = AssertEx.Throws<ArgumentException>(() => d.PrivateGeneric<int, int, int, int>(0, 0, 0));
            e3.Message.Is(s => s.Contains("not match arguments") && s.Contains("PrivateGeneric"));
        }

        [TestMethod]
        [Ignore]
        public void DynamicNotSupportedCase()
        {
            var d = new GenericPrivateMock().AsDynamic();

            (d.ReturnType(Enumerable.Range(1, 10), 0) as Type).Is(typeof(int));
            (d.ReturnType<int>(Enumerable.Range(1, 10), 0) as Type).Is(typeof(int));

            var dict = new Dictionary<int, IDictionary<string, double>>();
            (d.DictGen(dict, 1.9) as string).Is("dict");
            (d.DictGen<int, string, double>(dict, 1.9) as string).Is("dict");
        }

        // testcase

        [TestMethod]
        [TestCase(1, 2, 3)]
        [TestCase(10, 20, 30)]
        [TestCase(100, 200, 300)]
        public void TestCaseTest()
        {
            TestContext.Run((int x, int y, int z) =>
            {
                (x + y).Is(z);
                (x + y + z).Is(i => i < 1000);
            });
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
            ex.ParamName.Is("nullnull");
        }
    }
}