using System;
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

    [Test]
    public void Guid()
    {
        var target = new ClassWithGuid
        {
            Property = new Guid("45b14050-065c-4be7-8bb8-f3b46b8d94e6")
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("45b14050-065c-4be7-8bb8-f3b46b8d94e6", result.Property.ToString());
    }

    public class ClassWithGuid
    {
        [Encrypt]
        public Guid Property { get; set; }
    }
}