using System;
using System.Linq;
using MbUnit.Framework;
using System.Collections.Generic;

namespace ChainingAssertion
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void IsTest()
        {
            // "Is" extend on all object and has three overloads.

            // This same as Assert.AreEqual(25, Math.Pow(5, 2))
            Math.Pow(5, 2).Is(25);

            // lambda predicate assertion.
            // This same as Assert.IsTrue("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
            "foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));

            // has collection assert
            // This same as Assert.AreElementsEqual(Enumerable.Range(1,5), new[]{1, 2, 3, 4, 5})
            Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);
        }

        [Test]
        public void CollectionTest()
        {
            // if you want to use AreElementsEqual Methods then use Linq to Objects and Is
            new[] { 1, 3, 7, 8 }.Contains(8).Is(true);
            new[] { 1, 3, 7, 8 }.Count(i => i % 2 != 0).Is(3);
            new[] { 1, 3, 7, 8 }.Any().Is(true);
            new[] { 1, 3, 7, 8 }.All(i => i < 5).Is(false);

            // IsOrdered
            var array = new[] { 1, 5, 10, 100 };
            array.Is(array.OrderBy(x => x));
        }

        [Test]
        public void OthersTest()
        {
            // Null Assertions
            Object obj = null;
            obj.IsNull();             // Assert.IsNull(obj)
            new Object().IsNotNull(); // Assert.IsNotNull(obj)

            // Not Assertion
            "foobar".IsNot("fooooooo"); // Assert.AreNotEqual
            new[] { "a", "z", "x" }.IsNot("a", "x", "z"); // Assert.AreElementsNotEqual

            // ReferenceEqual Assertion
            var tuple = Tuple.Create("foo");
            tuple.IsSameReferenceAs(tuple); // Assert.AreSame
            tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.AreNotSame

            // Type Assertion
            "foobar".IsInstanceOf<string>(); // Assert.IsInstanceOfType
            (999).IsNotInstanceOf<double>(); // Assert.IsNotInstanceOfType
        }

        [Test]
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

        [Test]
        public void ExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => "foo".StartsWith(null));

            Assert.DoesNotThrow(() =>
            {
                // code
            });
        }

        [Test]
        [Row(1, 2, 3)]
        [Row(10, 20, 30)]
        [Row(100, 200, 300)]
        public void TestCaseTest(int x, int y, int z)
        {
            (x + y).Is(z);
            (x + y + z).Is(i => i < 1000);
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

        [Test]
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

            var e1 = Assert.Throws<ArgumentException>(() => { var x = d["hoge"]; });
            e1.Message.Is(s => s.Contains("indexer not found"));

            var e2 = Assert.Throws<ArgumentException>(() => { d["hoge"] = "a"; });
            e2.Message.Is(s => s.Contains("indexer not found"));
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

        [Test]
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

        [Test]
        public void GenericPrivateExceptionTest()
        {
            var d = new GenericPrivateMock().AsDynamic();

            var e1 = Assert.Throws<ArgumentException>(() => d.HogeHoge());
            e1.Message.Is(s => s.Contains("not found") && s.Contains("HogeHoge"));

            var e2 = Assert.Throws<ArgumentException>(() => d.PrivateGeneric(1));
            e2.Message.Is(s => s.Contains("not match arguments") && s.Contains("PrivateGeneric"));

            var e3 = Assert.Throws<ArgumentException>(() => d.PrivateGeneric<int, int, int, int>(0, 0, 0));
            e3.Message.Is(s => s.Contains("not match arguments") && s.Contains("PrivateGeneric"));
        }
    }
}