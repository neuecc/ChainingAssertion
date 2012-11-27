using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var foobar = default(object);
            foobar = "foobar";
            foobar.IsInstanceOf<string>() // Assert.IsInstanceOfType
                .ToUpper().Is("FOOBAR");            // ...returns the instance as TExpected.
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

            private bool NonParameterMethod()
            {
                return true;
            }

            private void VoidMethod(List<int> list)
            {
                list.Add(-100);
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

            ((bool)d.NonParameterMethod()).Is(true);
            var list = new List<int>();
            d.VoidMethod(list);
            list[0].Is(-100);
            list.Count.Is(1);

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

        private class Person
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

        void ContractRequires(string s)
        {
            System.Diagnostics.Contracts.Contract.Requires(s != null);
        }

        // for CodeContracts...
        // [TestMethod]
        public void ThrowsContractException()
        {
            AssertEx.ThrowsContractException(() => ContractRequires(null));

            AssertEx.Throws<AssertFailedException>(() =>
                AssertEx.ThrowsContractException(() => ContractRequires("a")));

            AssertEx.Throws<AssertFailedException>(() =>
                AssertEx.ThrowsContractException(() => { throw new Exception(); }));
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

        [TestMethod]
        public void IsNullMethodMessage()
        {
            object o = new object();
            o.IsNotNull();
            AssertEx.Throws<AssertFailedException>(
                () => o.IsNull("msg_msg"))
            .Message.Contains("msg_msg").Is(true);

            o = null;
            o.IsNull();
            AssertEx.Throws<AssertFailedException>(
                () => o.IsNotNull("msg_msg"))
            .Message.Contains("msg_msg").Is(true);
        }

        public class StructuralEqualTestClass
        {
            public int IntPro { get; set; }
            public string StrProp { get; set; }
            public int IntField;
            public string StrField;
            public int SetOnlyProp { private get; set; }
            public int[] IntArray { get; set; }
            public Stru StruStru;

            static Random rand = new Random();

            public StructuralEqualTestClass()
            {
                SetOnlyProp = rand.Next();
            }
        }

        public class DummyStructural : IEquatable<DummyStructural>
        {
            public string MyProperty { get; set; }

            public bool Equals(DummyStructural other)
            {
                return true;
            }
        }

        public class Stru
        {
            public int MyProperty { get; set; }
            public string[] StrArray { get; set; }
            public MMM MP2 { get; set; }
        }

        public class MMM
        {
            public int MyProperty { get; set; }
        }

        public class EmptyClass
        {

        }

        [TestMethod]
        public void StructuralEqualSuccess()
        {
            // primitive
            "hoge".IsStructuralEqual("hoge");
            (100).IsStructuralEqual(100);
            new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2, 3 });

            // complex
            new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } }.IsStructuralEqual(new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } });
            new DummyStructural() { MyProperty = "aiueo" }.IsStructuralEqual(new DummyStructural() { MyProperty = "kakikukeko" });
            new EmptyClass().IsStructuralEqual(new EmptyClass());

            var s1 = new StructuralEqualTestClass
            {
                IntPro = 1,
                IntField = 10,
                StrField = "hoge",
                StrProp = "huga",
                IntArray = new[] { 1, 2, 3, 4, 5 },
                StruStru = new Stru()
                {
                    MyProperty = 1000,
                    StrArray = new[] { "hoge", "huga", "tako" },
                    MP2 = new MMM() { MyProperty = 10000 }
                }
            };

            var s2 = new StructuralEqualTestClass
            {
                IntPro = 1,
                IntField = 10,
                StrField = "hoge",
                StrProp = "huga",
                IntArray = new[] { 1, 2, 3, 4, 5 },
                StruStru = new Stru()
                {
                    MyProperty = 1000,
                    StrArray = new[] { "hoge", "huga", "tako" },
                    MP2 = new MMM() { MyProperty = 10000 }
                }
            };

            s1.IsStructuralEqual(s1);
            s1.IsStructuralEqual(s2);
        }

        [TestMethod]
        public void StructuralEqualFailed()
        {
            // type
            object n = null;
            AssertEx.Throws<AssertFailedException>(() => n.IsStructuralEqual("a"));
            AssertEx.Throws<AssertFailedException>(() => "a".IsStructuralEqual(n));
            int i = 10;
            long l = 10;
            AssertEx.Throws<AssertFailedException>(() => i.IsStructuralEqual(l));

            // primitive
            AssertEx.Throws<AssertFailedException>(() => "hoge".IsStructuralEqual("hage"))
                .Message.Contains("actual = hoge expected = hage").Is(true);
            AssertEx.Throws<AssertFailedException>(() => (100).IsStructuralEqual(101))
                .Message.Contains("actual = 100 expected = 101").Is(true);

            AssertEx.Throws<AssertFailedException>(() => new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2 }))
                .Message.Contains("actual = 3 expected = ").Is(true);

            AssertEx.Throws<AssertFailedException>(() => new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2, 4 }))
                .Message.Contains("actual = 3 expected = 4").Is(true);

            AssertEx.Throws<AssertFailedException>(() => new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2, 3, 4 }))
                .Message.Contains("actual =  expected = 4").Is(true);

            AssertEx.Throws<AssertFailedException>(() => new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } }.IsStructuralEqual(new { Hoge = "aiueo", Huga = 100, Tako = new { k = 12 } }))
                .Message.Contains("actual = 10 expected = 12").Is(true);

            var s1 = new StructuralEqualTestClass
            {
                IntPro = 1,
                IntField = 10,
                StrField = "hoge",
                StrProp = "huga",
                IntArray = new[] { 1, 2, 3, 4, 5 },
                StruStru = new Stru()
                {
                    MyProperty = 1000,
                    StrArray = new[] { "hoge", "huga", "tako" },
                    MP2 = new MMM() { MyProperty = 10000 }
                }
            };

            var s2 = new StructuralEqualTestClass
            {
                IntPro = 1,
                IntField = 10,
                StrField = "hoge",
                StrProp = "huga",
                IntArray = new[] { 1, 2, 3, 4, 5, 6 },
                StruStru = new Stru()
                {
                    MyProperty = 1000,
                    StrArray = new[] { "hoge", "huga", "tako" },
                    MP2 = new MMM() { MyProperty = 10000 }
                }
            };

            var s3 = new StructuralEqualTestClass
            {
                IntPro = 1,
                IntField = 10,
                StrField = "hoge",
                StrProp = "huga",
                IntArray = new[] { 1, 2, 3, 4, 5 },
                StruStru = new Stru()
                {
                    MyProperty = 1000,
                    StrArray = new[] { "hoge", "huga", "tako" },
                    MP2 = new MMM() { MyProperty = 13000 }
                }
            };

            AssertEx.Throws<AssertFailedException>(() => s1.IsStructuralEqual(s2))
                .Message.Contains("StructuralEqualTestClass.IntArray.[5]").Is(true);

            AssertEx.Throws<AssertFailedException>(() => s1.IsStructuralEqual(s3))
                .Message.Contains("StructuralEqualTestClass.StruStru.MP2.MyProperty").Is(true);
        }
    }
}