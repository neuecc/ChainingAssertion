/*--------------------------------------------------------------------------
 * Chaining Assertion
 * ver 1.2.0.0 (Mar. 3rd, 2011)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * http://chainingassertion.codeplex.com/
 *--------------------------------------------------------------------------*/

at first, include .cs file on your UnitTesting Project.

ChainingAssertion.MSTest.cs - MSTest
ChainingAssertion.NUnit.cs - NUnit
ChainingAssertion.MbUnit.cs - MbUnit(Gallio)
ChainingAssertion.xUnit.cs - xUnit.net

following tutorial is for MSTest.
other version, see .cs's header.

| three example, "Is" overloads.

// This same as Assert.AreEqual(25, Math.Pow(5, 2))
Math.Pow(5, 2).Is(25);

// This same as Assert.IsTrue("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
"foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));

// This same as CollectionAssert.AreEqual(Enumerable.Range(1,5), new[]{1, 2, 3, 4, 5})
Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);

| CollectionAssert
| if you want to use CollectionAssert Methods then use Linq to Objects and Is

new[] { 1, 3, 7, 8 }.Contains(8).Is(true);
new[] { 1, 3, 7, 8 }.Count(i => i % 2 != 0).Is(3);
new[] { 1, 3, 7, 8 }.Any().Is(true);
new[] { 1, 3, 7, 8 }.All(i => i < 5).Is(false);

// IsOrdered
var array = new[] { 1, 5, 10, 100 };
array.Is(array.OrderBy(x => x));

| Other Assertions

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

| Advanced Collection Assertion

var lower = new[] { "a", "b", "c" };
var upper = new[] { "A", "B", "C" };

// Comparer CollectionAssert, use IEqualityComparer<T> or Func<T,T,bool> delegate
lower.Is(upper, StringComparer.InvariantCultureIgnoreCase);
lower.Is(upper, (x, y) => x.ToUpper() == y.ToUpper());

// or you can use Linq to Objects - SequenceEqual
lower.SequenceEqual(upper, StringComparer.InvariantCultureIgnoreCase).Is(true);

| Exception Test

// Exception Test(alternative of ExpectedExceptionAttribute)
AssertEx.Throws<ArgumentNullException>(() => "foo".StartsWith(null));

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

| Parameterized Test
| TestCase takes parameters and send to TestContext's Extension Method "Run".

[TestClass]
public class UnitTest
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [TestCase(1, 2, 3)]
    [TestCase(10, 20, 30)]
    [TestCase(100, 200, 300)]
    public void TestMethod2()
    {
        TestContext.Run((int x, int y, int z) =>
        {
            (x + y).Is(z);
            (x + y + z).Is(i => i < 1000);
        });
    }
}

| TestCaseSource
| TestCaseSource can take static field/property that types is only object[][].

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

-- History --
2011-03-03 ver 1.2.0.0
    Add Methods
      AssertEx.Throws, AssertEx.DoesNotThrow, Is(EqualityComparer overload)
    Add Files
      NUnit, xUnit.NET, MbUnit version.

2011-02-28 ver 1.1.0.1
    Fix Bugs - IsNot

2011-02-28 ver 1.1.0.0
    Add Methods
      IsNot, IsInstanceOf, IsNotInstanceOf, IsSameReferenceAs, IsNotSameReferenceAs

2011-02-22 ver 1.0.0.0
    1st Release