using System.Collections.Generic;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class DictionaryUsageUsage
{
    [Test]
    public void ByteArrayDictionary()
    {
        var target = new ClassWithByteArrayDictionary
        {
            Property = new Dictionary<string, byte[]>
            {
                {
                    "Key1", new byte[] {2, 3}
                },
                {
                    "Key2", new byte[] {5, 6}
                }
            }
        };
        var result = RoundTrip.Run(target);
        CollectionAssert.AreEqual(new byte[] {5, 6}, result.Property["Key2"]);
    }

    public class ClassWithByteArrayDictionary
    {
        [Encrypt]
        public Dictionary<string, byte[]> Property { get; set; }
    }

    [Test]
    public void StringDictionary()
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
    public void IntStringDictionary()
    {
        var target = new ClassWithIntStringDictionary
        {
            Property = new Dictionary<int, string>
            {
                {
                    1, "Value1"
                },
                {
                    2, "Value2"
                }
            }
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("Value2", result.Property[2]);
    }

    public class ClassWithIntStringDictionary
    {
        [Encrypt]
        public Dictionary<int, string> Property { get; set; }
    }
}