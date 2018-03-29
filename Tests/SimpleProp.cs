using System;
using Newtonsoft.Json.Encryption;
using Xunit;
using Xunit.Abstractions;

public class SimpleProp: TestBase
{
    public SimpleProp(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ByteArray()
    {
        var target = new ClassWithByteArray
        {
            Property = new byte[]{2,3}
        };
        var result = RoundTrip.Run(target);
        Assert.Equal(new byte[] { 2, 3 }, result.Property);
    }

    public class ClassWithByteArray
    {
        [Encrypt]
        public byte[] Property { get; set; }
    }

    [Fact]
    public void String()
    {
        var target = new ClassWithString
        {
            Property = "Foo"
        };
        var result = RoundTrip.Run(target);
        Assert.Equal("Foo", result.Property);
    }

    public class ClassWithString
    {
        [Encrypt]
        public string Property { get; set; }
    }

    [Fact]
    public void Guid()
    {
        var target = new ClassWithGuid
        {
            Property = new Guid("45b14050-065c-4be7-8bb8-f3b46b8d94e6")
        };
        var result = RoundTrip.Run(target);
        Assert.Equal("45b14050-065c-4be7-8bb8-f3b46b8d94e6", result.Property.ToString());
    }

    public class ClassWithGuid
    {
        [Encrypt]
        public Guid Property { get; set; }
    }
}