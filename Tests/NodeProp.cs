using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class NodeProp
{
    [Test]
    public void Node()
    {
        var target = new TargetClass
        {
            SubProperty = new SubClass
            {
                Property1 = "PropertyValue1",
                Property2 = "PropertyValue2"
            }
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("PropertyValue1", result.SubProperty.Property1);
    }

    public class TargetClass
    {
        [NodeEncrypt]
        public SubClass SubProperty { get; set; }
    }

    public class SubClass
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }

    [Test]
    public void NodeWithConverter()
    {
        var target = new WithConverterTargetClass
        {
            Property = "AbCd"
        };
        var result = RoundTrip.Run(target);
        Assert.AreEqual("AbCd", result.Property);
        ReverseConverter.AssertReadWriteCalled();
    }

    public class WithConverterTargetClass
    {
        [NodeEncrypt]
        [JsonConverter(typeof(ReverseConverter))]
        public string Property { get; set; }
    }
}