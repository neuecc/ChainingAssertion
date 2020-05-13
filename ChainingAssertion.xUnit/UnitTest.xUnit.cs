using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

#if DEBUG
namespace ChainingAssertion
{
    public class UnitTest
    {
        [Fact]
        public void IsTest()
        {
            // "Is" extend on all object and has three overloads.

            // This same as Assert.Equal(25, Math.Pow(5, 2))
            Math.Pow(5, 2).Is(25);

            // lambda predicate assertion.
            // This same as Assert.True("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
            "foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));

            // has collection assert
            // This same as Assert.Equal(Enumerable.Range(1,5).ToArray(), new[]{1, 2, 3, 4, 5}.ToArray())
            // it is sequence value compare
            Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);
        }

        [Fact]
        public void CollectionTest()
        {
            // if you want to use CollectionAssert Methods then use Linq to Objects and Is
            new[] { 1, 3, 7, 8 }.Contains(8).Is(true);
            new[] { 1, 3, 7, 8 }.Count(i => i % 2 != 0).Is(3);
            new[] { 1, 3, 7, 8 }.Any().Is(true);
            new[] { 1, 3, 7, 8 }.All(i => i < 5).Is(false);

            // IsOrdered
            var array = new[] { 1, 5, 10, 100 };
            array.Is(array.OrderBy(x => x).ToArray());
        }

        [Fact]
        public void OthersTest()
        {
            // Null Assertions
            Object obj = null;
            obj.IsNull();             // Assert.Null(obj)
            new Object().IsNotNull(); // Assert.NotNull(obj)

            // Not Assertion
            "foobar".IsNot("fooooooo"); // Assert.NotEqual
            new[] { "a", "z", "x" }.IsNot("a", "x", "z"); /// Assert.NotEqual

            // ReferenceEqual Assertion
            var tuple = Tuple.Create("foo");
            tuple.IsSameReferenceAs(tuple); // Assert.Same
            tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.NotSame

            // Type Assertion
            "foobar".IsInstanceOf<string>(); // Assert.IsType
            (999).IsNotInstanceOf<double>(); // Assert.IsNotType
        }

        [Fact]
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

        [Fact]
        public void ExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => "foo".StartsWith(null));
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(10, 20, 30)]
        [InlineData(100, 200, 300)]
        public void TestCaseTest(int x, int y, int z)
        {

            (x + y).Is(z);
            (x + y + z).Is(i => i < 1000);
        }

        [Theory]
        [MemberData("toaruSource")]
        public void TestTestCaseSource(int x, int y, string z)
        {
            string.Concat(x, y).Is(z);
        }

        public static object[] toaruSource
        {
            get
            {
                return new[]
                {
                    new object[] {1, 1, "11"},
                    new object[] {5, 3, "53"},
                    new object[] {9, 4, "94"}
                };
            }
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

        [Fact]
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

            ((string)d[100.101]).Is(100.101.ToString());
            d[72] = "Chihaya";
            (d.privateField as string).Is("Chihaya72");

            ((bool)d.NonParameterMethod()).Is(true);
            var list = new List<int>();
            d.VoidMethod(list);
            list[0].Is(-100);
            list.Count.Is(1);

            var e1 = Assert.Throws<ArgumentException>(() => { var x = d["hoge"]; });
            e1.Message.Is(s => s.Contains("indexer not found"));

            var e2 = Assert.Throws<ArgumentException>(() => { d["hoge"] = "a"; });
            e2.Message.Is(s => s.Contains("indexer not found"));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        private class Person
        {
            public int Age { get; set; }
            public string FamilyName { get; set; }
            public string GivenName { get; set; }
        }

        [Fact]
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
            Assert.True(false);
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

        [Fact]
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

        [Fact]
        public void StructuralEqualFailed()
        {
            // type
            object n = null;
            Assert.Throws<ArgumentNullException>(() => n.IsStructuralEqual("a"));
            Assert.Throws<ArgumentNullException>(() => "a".IsStructuralEqual(n));
            int i = 10;
            long l = 10;
            Assert.Throws<XunitException>(() => i.IsStructuralEqual(l));

            // primitive
            Assert.Throws<XunitException>(() => "hoge".IsStructuralEqual("hage"))
                .Message.Contains("actual = hoge expected = hage").Is(true);
            Assert.Throws<XunitException>(() => (100).IsStructuralEqual(101))
                .Message.Contains("actual = 100 expected = 101").Is(true);

            Assert.Throws<XunitException>(() => new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2 }))
                .Message.Contains("actual = 3 expected = ").Is(true);

            Assert.Throws<XunitException>(() => new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2, 4 }))
                .Message.Contains("actual = 3 expected = 4").Is(true);

            Assert.Throws<XunitException>(() => new[] { 1, 2, 3 }.IsStructuralEqual(new[] { 1, 2, 3, 4 }))
                .Message.Contains("actual =  expected = 4").Is(true);

            Assert.Throws<XunitException>(() => new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } }.IsStructuralEqual(new { Hoge = "aiueo", Huga = 100, Tako = new { k = 12 } }))
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

            Assert.Throws<XunitException>(() => s1.IsStructuralEqual(s2))
                .Message.Contains("StructuralEqualTestClass.IntArray.[5]").Is(true);

            Assert.Throws<XunitException>(() => s1.IsStructuralEqual(s3))
                .Message.Contains("StructuralEqualTestClass.StruStru.MP2.MyProperty").Is(true);
        }


        [Fact]
        public void NotStructuralEqualFailed()
        {
            // primitive
            Assert.Throws<XunitException>(() => "hoge".IsNotStructuralEqual("hoge"));
            Assert.Throws<XunitException>(() => (100).IsNotStructuralEqual(100));
            Assert.Throws<XunitException>(() => new[] { 1, 2, 3 }.IsNotStructuralEqual(new[] { 1, 2, 3 }));

            // complex
            Assert.Throws<XunitException>(() => new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } }.IsNotStructuralEqual(new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } }));
            Assert.Throws<XunitException>(() => new DummyStructural() { MyProperty = "aiueo" }.IsNotStructuralEqual(new DummyStructural() { MyProperty = "kakikukeko" }));
            Assert.Throws<XunitException>(() => new EmptyClass().IsNotStructuralEqual(new EmptyClass()));

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

            Assert.Throws<XunitException>(() => s1.IsNotStructuralEqual(s1));
            Assert.Throws<XunitException>(() => s1.IsNotStructuralEqual(s2));
        }

        [Fact]
        public void NotStructuralEqualSuccess()
        {
            // type
            object n = null;
            n.IsNotStructuralEqual("a");
            "a".IsNotStructuralEqual(n);
            int i = 10;
            long l = 10;
            i.IsNotStructuralEqual(l);

            // primitive
            "hoge".IsNotStructuralEqual("hage");
            (100).IsNotStructuralEqual(101);

            new[] { 1, 2, 3 }.IsNotStructuralEqual(new[] { 1, 2 });

            new[] { 1, 2, 3 }.IsNotStructuralEqual(new[] { 1, 2, 4 });

            new[] { 1, 2, 3 }.IsNotStructuralEqual(new[] { 1, 2, 3, 4 });

            new { Hoge = "aiueo", Huga = 100, Tako = new { k = 10 } }.IsNotStructuralEqual(new { Hoge = "aiueo", Huga = 100, Tako = new { k = 12 } });

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

            s1.IsNotStructuralEqual(s2);

            s1.IsNotStructuralEqual(s3);
        }
    }
}
#endif
