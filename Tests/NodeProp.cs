using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using Xunit;
using Xunit.Abstractions;

public class NodeProp: TestBase
{
    public NodeProp(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
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
        Assert.Equal("PropertyValue1", result.SubProperty.Property1);
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

    [Fact]
    public void NodeWithConverter()
    {
        var target = new WithConverterTargetClass
        {
            Property = "AbCd"
        };
        var result = RoundTrip.Run(target);
        Assert.Equal("AbCd", result.Property);
        ReverseConverter.AssertReadWriteCalled();
    }

    public class WithConverterTargetClass
    {
        [NodeEncrypt]
        [JsonConverter(typeof(ReverseConverter))]
        public string Property { get; set; }
    }
}