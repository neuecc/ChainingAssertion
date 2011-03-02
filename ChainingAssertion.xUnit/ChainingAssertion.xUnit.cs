/*--------------------------------------------------------------------------
 * Chaining Assertion for xUnit
 * ver 1.1.0.1 (Feb. 28th, 2011)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * http://chainingassertion.codeplex.com/
 *--------------------------------------------------------------------------*/

// TODO:Write Turotials

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
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
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

        /// <summary>Assert.NotEqual, if T is IEnumerable then check value equality</summary>
        public static void IsNot<T>(this T actual, T expected)
        {
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
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
    }

    #endregion
}