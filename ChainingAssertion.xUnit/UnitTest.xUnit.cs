using System;
using System.Linq;
using Xunit;
using Xunit.Extensions;

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

            Assert.DoesNotThrow(() =>
            {
                // code
            });
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
        [PropertyData("toaruSource")]
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
    }
}