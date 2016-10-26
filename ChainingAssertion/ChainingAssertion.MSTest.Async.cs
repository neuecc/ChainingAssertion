using System;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static partial class TestContextExtensions
    {
        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1>(this TestContext context, Func<T1, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2>(this TestContext context, Func<T1, T2, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3>(this TestContext context, Func<T1, T2, T3, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4>(this TestContext context, Func<T1, T2, T3, T4, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5>(this TestContext context, Func<T1, T2, T3, T4, T5, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9],
                    (T11)parameters[10]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9],
                    (T11)parameters[10],
                    (T12)parameters[11]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9],
                    (T11)parameters[10],
                    (T12)parameters[11],
                    (T13)parameters[12]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9],
                    (T11)parameters[10],
                    (T12)parameters[11],
                    (T13)parameters[12],
                    (T14)parameters[13]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9],
                    (T11)parameters[10],
                    (T12)parameters[11],
                    (T13)parameters[12],
                    (T14)parameters[13],
                    (T15)parameters[14]
                    );
            }
        }

        /// <summary>Run Parameterized Test marked by TestCase Attribute.</summary>
        public static async Task RunAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this TestContext context, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> assertion)
        {
            var type = Type.GetType(context.FullyQualifiedTestClassName);
            foreach (var parameters in GetParameters(type, context.TestName))
            {
                await assertion(
                    (T1)parameters[0],
                    (T2)parameters[1],
                    (T3)parameters[2],
                    (T4)parameters[3],
                    (T5)parameters[4],
                    (T6)parameters[5],
                    (T7)parameters[6],
                    (T8)parameters[7],
                    (T9)parameters[8],
                    (T10)parameters[9],
                    (T11)parameters[10],
                    (T12)parameters[11],
                    (T13)parameters[12],
                    (T14)parameters[13],
                    (T15)parameters[14],
                    (T16)parameters[15]
                    );
            }
        }
    }
}
