using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class AsyncUsage
{
    [Test]
    public async Task Works()
    {
        var target = new SimpleUsage.ClassWithString
        {
            Property = "Foo"
        };
        var result = await RoundTrip.RunAsync(target);
        Assert.AreEqual("Foo", result.Property);
    }
}