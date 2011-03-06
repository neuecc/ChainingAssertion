/*--------------------------------------------------------------------------
 * Chaining Assertion for MbUnit
 * ver 1.3.0.0 (Mar. 6th, 2011)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * http://chainingassertion.codeplex.com/
 *--------------------------------------------------------------------------*/

/* -- Tutorial --
 * | at first, include this file on MbUnit Project.
 * 
 * | three example, "Is" overloads.
 * 
 * // This same as Assert.AreEqual(25, Math.Pow(5, 2))
 * Math.Pow(5, 2).Is(25);
 * 
 * // This same as Assert.IsTrue("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
 * "foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));
 * 
 * // This same as Assert.AreElementsEqual(Enumerable.Range(1,5), new[]{1, 2, 3, 4, 5})
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
 * obj.IsNull();             // Assert.IsNull(obj)
 * new Object().IsNotNull(); // Assert.IsNotNull(obj)
 *
 * // Not Assertion
 * "foobar".IsNot("fooooooo"); // Assert.AreNotEqual
 * new[] { "a", "z", "x" }.IsNot("a", "x", "z"); // Assert.AreElementsNotEqual
 *
 * // ReferenceEqual Assertion
 * var tuple = Tuple.Create("foo");
 * tuple.IsSameReferenceAs(tuple); // Assert.AreSame
 * tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.AreNotSame
 *
 * // Type Assertion
 * "foobar".IsInstanceOf<string>(); // Assert.IsInstanceOfType
 * (999).IsNotInstanceOf<double>(); // Assert.IsNotInstanceOfType
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gallio.Common;

namespace MbUnit.Framework
{
    #region Extensions

    public static partial class AssertEx
    {
        /// <summary>Assert.AreEqual, if T is IEnumerable then Assert.AreElementsEqual</summary>
        public static void Is<T>(this T actual, T expected, string message = "")
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().Is(((IEnumerable)expected).Cast<object>(), message);
                return;
            }

            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>Assert.IsTrue(predicate(value))</summary>
        public static void Is<T>(this T value, Expression<System.Func<T, bool>> predicate, string message = "")
        {
            var paramName = predicate.Parameters.First().Name;
            var msg = string.Format("{0} = {1}, {2}{3}",
                paramName, value, predicate,
                string.IsNullOrEmpty(message) ? "" : ", " + message);

            Assert.IsTrue(predicate.Compile().Invoke(value), msg);
        }

        /// <summary>Assert.AreElementsEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, params T[] expected)
        {
            Is(actual, expected.AsEnumerable());
        }

        /// <summary>Assert.AreElementsEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message = "")
        {
            Assert.AreElementsEqual(expected, actual);
        }

        /// <summary>Assert.AreElementsEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message = "")
        {
            Assert.AreElementsEqual(expected, actual, comparer, message);
        }

        /// <summary>Assert.AreElementsEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, EqualityComparison<T> comparison, string message = "")
        {
            Assert.AreElementsEqual(expected, actual, comparison, message);
        }

        /// <summary>Assert.AreNotEqual, if T is IEnumerable then Assert.AreElementsNotEqual</summary>
        public static void IsNot<T>(this T actual, T notExpected, string message = "")
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().IsNot(((IEnumerable)notExpected).Cast<object>(), message);
                return;
            }

            Assert.AreNotEqual(notExpected, actual, message);
        }

        /// <summary>Assert.AreElementsNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, params T[] notExpected)
        {
            IsNot(actual, notExpected.AsEnumerable());
        }

        /// <summary>Assert.AreElementsNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, string message = "")
        {
            Assert.AreElementsNotEqual(notExpected.ToArray(), actual.ToArray(), message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, IEqualityComparer<T> comparer, string message = "")
        {
            Assert.AreElementsNotEqual(notExpected, actual, comparer, message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, EqualityComparison<T> comparison, string message = "")
        {
            Assert.AreElementsNotEqual(notExpected, actual, comparison, message);
        }

        /// <summary>Assert.IsNull</summary>
        public static void IsNull<T>(this T value)
        {
            Assert.IsNull(value);
        }

        /// <summary>Assert.IsNotNull</summary>
        public static void IsNotNull<T>(this T value)
        {
            Assert.IsNotNull(value);
        }

        /// <summary>Assert.AreSame</summary>
        public static void IsSameReferenceAs<T>(this T actual, T expected, string message = "")
            where T : class
        {
            Assert.AreSame(expected, actual, message);
        }

        /// <summary>Assert.AreNotSame</summary>
        public static void IsNotSameReferenceAs<T>(this T actual, T notExpected, string message = "")
            where T : class
        {
            Assert.AreNotSame(notExpected, actual, message);
        }

        /// <summary>Assert.IsInstanceOfType</summary>
        public static void IsInstanceOf<TExpected>(this object value, string message = "")
        {
            Assert.IsInstanceOfType<TExpected>(value, message);
        }

        /// <summary>Assert.IsNotInstanceOfType</summary>
        public static void IsNotInstanceOf<TWrong>(this object value, string message = "")
        {
            Assert.IsNotInstanceOfType<TWrong>(value, message);
        }
    }

    #endregion
}