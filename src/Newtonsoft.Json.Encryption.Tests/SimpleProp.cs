using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Encryption;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class SimpleProp :
    VerifyBase
{
    public SimpleProp(ITestOutputHelper output) :
        base(output)
    {
    }

    [Fact]
    public async Task ByteArray()
    {
        var target = new ClassWithByteArray
        {
            Property = new byte[]{2,3}
        };
        var result = await this.Run(target);
        Assert.Equal(new byte[] { 2, 3 }, result.Property);
    }

    public class ClassWithByteArray
    {
        [Encrypt]
        public byte[]? Property { get; set; }
    }

    [Fact]
    public async Task NullString()
    {
        var target = new ClassWithString();
        var result = await this.Run(target);
        Assert.Null(result.Property);
    }

    [Fact]
    public async Task String()
    {
        var target = new ClassWithString
        {
            Property = "Foo"
        };
        var result = await this.Run(target);
        Assert.Equal("Foo", result.Property);
    }

    [Fact]
    public async Task EmptyString()
    {
        var target = new ClassWithString
        {
            Property = string.Empty
        };
        var result = await this.Run(target);
        Assert.Empty(result.Property);
    }

    public class ClassWithString
    {
        [Encrypt]
        public string? Property { get; set; }
    }

    [Fact]
    public async Task Guid()
    {
        var target = new ClassWithGuid
        {
            Property = new Guid("45b14050-065c-4be7-8bb8-f3b46b8d94e6")
        };
        var result = await this.Run(target);
        Assert.Equal("45b14050-065c-4be7-8bb8-f3b46b8d94e6", result.Property.ToString());
    }

    public class ClassWithGuid
    {
        [Encrypt]
        public Guid Property { get; set; }
    }
}