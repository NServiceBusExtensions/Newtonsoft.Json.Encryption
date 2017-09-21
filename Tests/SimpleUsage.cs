using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class SimpleUsage
{
    [Test]
    public void ByteArrayProperty()
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
    public void ByteArrayDictionaryProperty()
    {
        var target = new ClassWithByteArrayDictionary
        {
            Property = new Dictionary<string, byte[]>
            {
                {
                    "Key1", new byte[]{2,3}
                },
                {
                    "Key2", new byte[]{5,6}
                }
            }
        };
        var result = RoundTrip.Run(target);
        CollectionAssert.AreEqual(new byte[] { 5, 6 }, result.Property["Key2"]);
    }

    public class ClassWithByteArrayDictionary
    {
        [Encrypt]
        public Dictionary<string, byte[]> Property { get; set; }
    }

    [Test]
    public void ByteArrayListProperty()
    {
        var target = new ClassWithByteArrayList
        {
            Property = new List<byte[]>
            {
                new byte[]{2,3},
                new byte[]{5,6}
            }
        };
        var result = RoundTrip.Run(target);
        CollectionAssert.AreEqual(new byte[] { 5, 6 }, result.Property[1]);
    }

    public class ClassWithByteArrayList
    {
        [Encrypt]
        public List<byte[]> Property { get; set; }
    }

    [Test]
    public void StringProperty()
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
    public void StringDictionaryProperty()
    {
        var target = new ClassWithStringDictionary
        {
            Property = new Dictionary<string, string>
            {
                {
                    "Key1", "Value1"
                },
                {
                    "Key2", "Value2"
                }
            }
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("Value2", result.Property["Key2"]);
    }

    public class ClassWithStringDictionary
    {
        [Encrypt]
        public Dictionary<string, string> Property { get; set; }
    }

    [Test]
    public void StringListProperty()
    {
        var target = new ClassWithStringList
        {
            Property = new List<string>
            {
                "Value1",
                "Value2"
            }
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("Value2", result.Property[1]);
    }

    public class ClassWithStringList
    {
        [Encrypt]
        public List<string> Property { get; set; }
    }

    [Test]
    public void StringCollectionProperty()
    {
        var target = new ClassWithStringCollection
        {
            Property = new List<string>
            {
                "Value1",
                "Value2"
            }
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("Value2", result.Property.Last());
    }

    public class ClassWithStringCollection
    {
        [Encrypt]
        public ICollection<string> Property { get; set; }
    }

    [Test]
    public void StringEnumerableProperty()
    {
        var target = new ClassWithStringEnumerable
        {
            Property = new List<string>
            {
                "Value1",
                "Value2"
            }
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("Value2", result.Property.Last());
    }

    public class ClassWithStringEnumerable
    {
        [Encrypt]
        public IEnumerable<string> Property { get; set; }
    }
}