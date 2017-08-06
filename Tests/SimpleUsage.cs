using System.Collections.Generic;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class SimpleUsage
{
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
}