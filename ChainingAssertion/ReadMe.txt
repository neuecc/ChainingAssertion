/*--------------------------------------------------------------------------
 * Chaining Assertion for MSTest
 * ver 1.0.0.0 (Feb. 22th, 2011)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * http://chainingassertion.codeplex.com/
 *--------------------------------------------------------------------------*/

-- Tutorial --
| at first, include this file on MSTest Project.

| three example, "Is" overloads.

// This same as Assert.AreEqual(25, Math.Pow(5, 2))
Math.Pow(5, 2).Is(25);

// This same as Assert.IsTrue("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
"foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));

// This same as CollectionAssert.AreEqual(Enumerable.Range(1,5), new[]{1, 2, 3, 4, 5})
Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);

| and two extension methods.

Object obj = null;
obj.IsNull();    // Assert.IsNull(obj)
obj.IsNotNull(); // Assert.IsNotNull(obj)

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

2011-02-22 ver 1.0.0.0
    1st Release