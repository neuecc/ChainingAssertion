/*--------------------------------------------------------------------------
 * Chaining Assertion
 * ver 1.7.1.0 (Apr. 29th, 2013)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * https://github.com/neuecc/ChainingAssertion
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

var array = new[] { 1, 3, 7, 8 };
array.Count().Is(4);
array.Contains(8).IsTrue(); // IsTrue() == Is(true)
array.All(i => i < 5).IsFalse(); // IsFalse() == Is(false)
array.Any().Is(true);
new int[] { }.Any().Is(false);   // IsEmpty
array.OrderBy(x => x).Is(array); // IsOrdered

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

| StructuralEqual

class MyClass
{
    public int IntProp { get; set; }
    public string StrField;
}

var mc1 = new MyClass() { IntProp = 10, StrField = "foo" };
var mc2 = new MyClass() { IntProp = 10, StrField = "foo" };

mc1.IsStructuralEqual(mc2); // deep recursive value equality compare

mc1.IntProp = 20;
mc1.IsNotStructuralEqual(mc2);

| DynamicAccessor

// AsDynamic convert to "dynamic" that can call private method/property/field/indexer.

// a class and private field/property/method.
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
}

// call private property.
var actual = new PrivateMock().AsDynamic().PrivateProperty;
Assert.AreEqual("homuhomu", actual);

// dynamic can't invoke extension methods.
// if you want to invoke "Is" then cast type.
(new PrivateMock().AsDynamic().PrivateMethod(3) as string).Is("homuhomuhomu");

// set value
var mock = new PrivateMock().AsDynamic();
mock.PrivateProperty = "mogumogu";
(mock.privateField as string).Is("mogumogu");

| Exception Test

// Exception Test(alternative of ExpectedExceptionAttribute)
// AssertEx.Throws does not allow derived type
// AssertEx.Catch allows derived type
// AssertEx.ThrowsContractException catch only Code Contract's ContractException
AssertEx.Throws<ArgumentNullException>(() => "foo".StartsWith(null));
AssertEx.Catch<Exception>(() => "foo".StartsWith(null));
AssertEx.ThrowsContractException(() => // contract method //);

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
2013-04-29 ver 1.7.1.0
    Add DebuggerStepThroughtAttribute
        pull request from mikehibm, thanks
2012-11-28 ver 1.7.0.1
    Add Method
        IsTrue
        IsFalse
        IsStructuralEqual
        IsNotStructuralEqual
    Changed
        IsInstanceOf returns actual
    Delete
        Silverlight, Windows Phone 7 support

2011-10-24 ver 1.6.1.0
    Changed
        IsNull/IsNotNull Add message overload

2011-10-20 ver 1.6.0.2
    Changed
        Avoid namespace/classname conflict "Expression"

2011-09-22 ver 1.6.0.1
    Add New File
        ChainingAssertion.SL
        ChainingAssertion.WP7

2011-09-20 ver 1.6.0.0
    Add Method
        AssertEx.ThrowsContractException(for MSTest)
    New Feature
        Is(lambda)'s error message output parameter values

2011-07-25 ver 1.5.0.0
    Fix Bugs
        AsDynamic can't call when parameter is null
    New Feature
        MSTest's TestCase - can invoke from separeted dll
        AsDynamic - improvement of the precision of the method match

2011-03-17 ver 1.4.0.0
    Add Methods
        AsDynamic(to DynamicAccessor that can call private method/field/property/indexer)

2011-03-06 ver 1.3.0.0
    Add Methods
        AssertEx.Catch
    Fix Bugs
        Is/IsNot(IEnumerable,IEnumerable,IEqualityComparer)'s Assertion actual and expected turned over

2011-03-03 ver 1.2.0.0
    Add Methods
      AssertEx.Throws, AssertEx.DoesNotThrow, Is(EqualityComparer overload)
    Add Files
      NUnit, xUnit.NET, MbUnit version

2011-02-28 ver 1.1.0.1
    Fix Bugs - IsNot

2011-02-28 ver 1.1.0.0
    Add Methods
      IsNot, IsInstanceOf, IsNotInstanceOf, IsSameReferenceAs, IsNotSameReferenceAs

2011-02-22 ver 1.0.0.0
    1st Release