/*--------------------------------------------------------------------------
 * Chaining Assertion for NUnit
 * ver 1.2.0.0 (Mar. 2nd, 2011)
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

namespace NUnit.Framework
{
    #region Extensions

    public static partial class AssertEx
    {
        /// <summary>Assert.AreEqual, if T is IEnumerable then CollectionAssert.AreEqual</summary>
        public static void Is<T>(this T actual, T expected, string message = "")
        {
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().Is(((IEnumerable)expected).Cast<object>(), message);
                return;
            }

            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>Assert.IsTrue(predicate(value))</summary>
        public static void Is<T>(this T value, Expression<Func<T, bool>> predicate, string message = "")
        {
            var paramName = predicate.Parameters.First().Name;
            var msg = string.Format("{0} = {1}, {2}{3}",
                paramName, value, predicate,
                string.IsNullOrEmpty(message) ? "" : ", " + message);

            Assert.IsTrue(predicate.Compile().Invoke(value), msg);
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, params T[] expected)
        {
            Is(actual, expected.AsEnumerable());
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message = "")
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray(), message);
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IComparer<T> comparer, string message = "")
        {
            Is(expected, actual, comparer.Compare, message);
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Comparison<T> comparison, string message = "")
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray(), new ComparisonComparer<T>(comparison), message);
        }

        /// <summary>Assert.AreNotEqual, if T is IEnumerable then CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this T actual, T notExpected, string message = "")
        {
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().IsNot(((IEnumerable)notExpected).Cast<object>(), message);
                return;
            }

            Assert.AreNotEqual(notExpected, actual, message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, params T[] notExpected)
        {
            IsNot(actual, notExpected.AsEnumerable());
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected.ToArray(), actual.ToArray(), message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IComparer<T> comparer, string message = "")
        {
            IsNot(expected, actual, comparer.Compare, message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Comparison<T> comparison, string message = "")
        {
            CollectionAssert.AreNotEqual(expected.ToArray(), actual.ToArray(), new ComparisonComparer<T>(comparison), message);
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
        {
            Assert.AreSame(expected, actual, message);
        }

        /// <summary>Assert.AreNotSame</summary>
        public static void IsNotSameReferenceAs<T>(this T actual, T notExpected, string message = "")
        {
            Assert.AreNotSame(notExpected, actual, message);
        }

        /// <summary>Assert.IsInstanceOf</summary>
        public static void IsInstanceOf<TExpected>(this object value, string message = "")
        {
            Assert.IsInstanceOf<TExpected>(value, message);
        }

        /// <summary>Assert.IsNotInstanceOf</summary>
        public static void IsNotInstanceOf<TWrong>(this object value, string message = "")
        {
            Assert.IsNotInstanceOf<TWrong>(value, message);
        }

        /// <summary>Comparison to IComparer Converter for CollectionAssert</summary>
        private class ComparisonComparer<T> : IComparer
        {
            readonly Comparison<T> comparison;

            public ComparisonComparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            public int Compare(object x, object y)
            {
                return (comparison != null)
                    ? comparison((T)x, (T)y)
                    : object.Equals(x, y) ? 0 : -1;
            }
        }
    }

    #endregion
}
