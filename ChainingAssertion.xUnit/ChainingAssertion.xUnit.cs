/*--------------------------------------------------------------------------
 * Chaining Assertion for xUnit
 * ver 1.2.0.0 (Mar. 3rd, 2011)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * http://chainingassertion.codeplex.com/
 *--------------------------------------------------------------------------*/

/* -- Tutorial --
 * | at first, include this file on xUnit.net Project.
 * 
 * | three example, "Is" overloads.
 * 
 * // This same as Assert.Equal(25, Math.Pow(5, 2))
 * Math.Pow(5, 2).Is(25);
 * 
 * // This same as Assert.True("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
 * "foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));
 * 
 * // This same as Assert.Equal(Enumerable.Range(1,5).ToArray(), new[]{1, 2, 3, 4, 5}.ToArray())
 * // it is sequence value compare
 * Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);
 * 
 * | CollectionAssert
 * | if you want to use CollectionAssert Methods then use Linq to Objects and Is
 *
 * new[] { 1, 3, 7, 8 }.Contains(8).Is(true);
 * new[] { 1, 3, 7, 8 }.Count(i => i % 2 != 0).Is(3);
 * new[] { 1, 3, 7, 8 }.Any().Is(true);
 * new[] { 1, 3, 7, 8 }.All(i => i < 5).Is(false);
 *
 * // IsOrdered
 * var array = new[] { 1, 5, 10, 100 };
 * array.Is(array.OrderBy(x => x));
 *
 * | Other Assertions
 * 
 * // Null Assertions
 * Object obj = null;
 * obj.IsNull();             // Assert.Null(obj)
 * new Object().IsNotNull(); // Assert.NotNull(obj)
 *
 * // Not Assertion
 * "foobar".IsNot("fooooooo"); // Assert.NotEqual
 * new[] { "a", "z", "x" }.IsNot("a", "x", "z"); /// Assert.NotEqual
 *
 * // ReferenceEqual Assertion
 * var tuple = Tuple.Create("foo");
 * tuple.IsSameReferenceAs(tuple); // Assert.Same
 * tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.NotSame
 *
 * // Type Assertion
 * "foobar".IsInstanceOf<string>(); // Assert.IsType
 * (999).IsNotInstanceOf<double>(); // Assert.IsNotType
 * 
 * | Advanced Collection Assertion
 * 
 * var lower = new[] { "a", "b", "c" };
 * var upper = new[] { "A", "B", "C" };
 *
 * // Comparer CollectionAssert, use IEqualityComparer<T> or Func<T,T,bool> delegate
 * lower.Is(upper, StringComparer.InvariantCultureIgnoreCase);
 * lower.Is(upper, (x, y) => x.ToUpper() == y.ToUpper());
 *
 * // or you can use Linq to Objects - SequenceEqual
 * lower.SequenceEqual(upper, StringComparer.InvariantCultureIgnoreCase).Is(true);
 * 
 * -- more details see project home --*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Xunit
{
    #region Extensions

    public static partial class AssertEx
    {
        /// <summary>Assert.Equal, if T is IEnumerable then compare value equality</summary>
        public static void Is<T>(this T actual, T expected)
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                Assert.Equal(
                    ((IEnumerable)actual).Cast<object>().ToArray(),
                    ((IEnumerable)expected).Cast<object>().ToArray());
                return;
            }

            Assert.Equal(expected, actual);
        }

        /// <summary>Assert.True(predicate(value))</summary>
        public static void Is<T>(this T value, Expression<Func<T, bool>> predicate, string message = "")
        {
            var paramName = predicate.Parameters.First().Name;
            var msg = string.Format("{0} = {1}, {2}{3}",
                paramName, value, predicate,
                string.IsNullOrEmpty(message) ? "" : ", " + message);

            Assert.True(predicate.Compile().Invoke(value), msg);
        }

        /// <summary>Assert.Equal</summary>
        public static void Is<T>(this T actual, T expected, IEqualityComparer<T> comparer)
        {
            Assert.Equal(expected, actual, comparer);
        }

        /// <summary>Assert.Equal(sequence value compare)</summary>
        public static void Is<T>(this IEnumerable<T> actual, params T[] expected)
        {
            Is(actual, expected.AsEnumerable());
        }

        /// <summary>Assert.Equal(sequence value compare)</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.Equal(expected.ToArray(), actual.ToArray());
        }

        /// <summary>Assert.True(actual.SequenceEqual(expected, comparer))</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            Assert.True(actual.SequenceEqual(expected, comparer));
        }

        /// <summary>Assert.True(actual.SequenceEqual(expected, comparison))</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<T, T, bool> equalityComparison)
        {
            Assert.True(actual.SequenceEqual(expected, new EqualityComparer<T>(equalityComparison)));
        }

        /// <summary>Assert.NotEqual, if T is IEnumerable then check value equality</summary>
        public static void IsNot<T>(this T actual, T expected)
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                Assert.NotEqual(
                    ((IEnumerable)actual).Cast<object>().ToArray(),
                    ((IEnumerable)expected).Cast<object>().ToArray());
                return;
            }

            Assert.NotEqual(expected, actual);
        }

        /// <summary>Assert.NotEqual</summary>
        public static void IsNot<T>(this T actual, T expected, IEqualityComparer<T> comparer)
        {
            Assert.NotEqual(expected, actual, comparer);
        }

        /// <summary>Assert.NotEqual(sequence value compare)</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, params T[] expected)
        {
            IsNot(actual, expected.AsEnumerable());
        }

        /// <summary>Assert.NotEqual(sequence value compare)</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.NotEqual(expected.ToArray(), actual.ToArray());
        }

        /// <summary>Assert.False(actual.SequenceEqual(expected, comparer))</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            Assert.False(actual.SequenceEqual(expected, comparer));
        }

        /// <summary>Assert.False(actual.SequenceEqual(expected, comparison))</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<T, T, bool> equalityComparison)
        {
            Assert.False(actual.SequenceEqual(expected, new EqualityComparer<T>(equalityComparison)));
        }

        /// <summary>Assert.Null</summary>
        public static void IsNull<T>(this T value)
        {
            Assert.Null(value);
        }

        /// <summary>Assert.NotNull</summary>
        public static void IsNotNull<T>(this T value)
        {
            Assert.NotNull(value);
        }

        /// <summary>Assert.Same</summary>
        public static void IsSameReferenceAs<T>(this T actual, T expected)
        {
            Assert.Same(expected, actual);
        }

        /// <summary>Assert.NotSame</summary>
        public static void IsNotSameReferenceAs<T>(this T actual, T notExpected)
        {
            Assert.NotSame(notExpected, actual);
        }

        /// <summary>Assert.IsType</summary>
        public static void IsInstanceOf<TExpected>(this object value)
        {
            Assert.IsType<TExpected>(value);
        }

        /// <summary>Assert.IsNotType</summary>
        public static void IsNotInstanceOf<TWrong>(this object value)
        {
            Assert.IsNotType<TWrong>(value);
        }

        /// <summary>EqualityComparison to IEqualityComparer Converter for CollectionAssert</summary>
        private class EqualityComparer<T> : IEqualityComparer<T>
        {
            readonly Func<T, T, bool> comparison;

            public EqualityComparer(Func<T, T, bool> comparison)
            {
                this.comparison = comparison;
            }


            public bool Equals(T x, T y)
            {
                return (comparison != null)
                    ? comparison(x, y)
                    : object.Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }
    }

    #endregion
}