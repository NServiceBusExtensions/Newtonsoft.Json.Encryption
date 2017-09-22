using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class SimpleProp
{
    [Test]
    public void ByteArray()
    {
        var target = new ClassWithByteArray
        {
            Property = new byte[]{2,3}
        };
        var result = RoundTrip.Run(target);
        CollectionAssert.AreEqual(new byte[] { 2, 3 }, result.Property);
    }

    public class ClassWithByteArray
    {
        [Encrypt]
        public byte[] Property { get; set; }
    }

    [Test]
    public void String()
    {
        var target = new ClassWithString
        {
            Property = "Foo"
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("Foo", result.Property);
    }

    public class ClassWithString
    {
        [Encrypt]
        public string Property { get; set; }
    }
}