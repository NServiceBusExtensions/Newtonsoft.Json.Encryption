using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class CollectionUsage
{
    [Test]
    public void ByteArrayList()
    {
        var target = new ClassWithByteArrayList
        {
            Property = new List<byte[]>
            {
                new byte[] {2, 3},
                new byte[] {5, 6}
            }
        };
        var result = RoundTrip.Run(target);
        CollectionAssert.AreEqual(new byte[] {5, 6}, result.Property[1]);
    }

    public class ClassWithByteArrayList
    {
        [Encrypt]
        public List<byte[]> Property { get; set; }
    }

    [Test]
    public void StringList()
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
    public void StringCollection()
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
    public void StringEnumerable()
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