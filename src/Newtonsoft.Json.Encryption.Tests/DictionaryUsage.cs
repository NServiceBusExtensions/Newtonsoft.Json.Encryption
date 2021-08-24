using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Encryption;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class DictionaryUsage
{
    [Fact]
    public async Task ByteArrayDictionary()
    {
        var target = new ClassWithByteArrayDictionary
        {
            Property = new()
            {
                {
                    "Key1", new byte[] {2, 3}
                },
                {
                    "Key2", new byte[] {5, 6}
                }
            }
        };
        var result = await RoundTrip.Run(target);
        Assert.Equal(new byte[] {5, 6}, result.Property?["Key2"]);
    }

    public class ClassWithByteArrayDictionary
    {
        [Encrypt]
        public Dictionary<string, byte[]>? Property { get; set; }
    }

    [Fact]
    public async Task StringDictionary()
    {
        var target = new ClassWithStringDictionary
        {
            Property = new()
            {
                {
                    "Key1", "Value1"
                },
                {
                    "Key2", "Value2"
                }
            }
        };
        var result = await RoundTrip.Run(target);
        Assert.Equal("Value2", result.Property?["Key2"]);
    }

    public class ClassWithStringDictionary
    {
        [Encrypt]
        public Dictionary<string, string>? Property { get; set; }
    }

    [Fact]
    public async Task IntStringDictionary()
    {
        var target = new ClassWithIntStringDictionary
        {
            Property = new()
            {
                {
                    1, "Value1"
                },
                {
                    2, "Value2"
                }
            }
        };
        var result = await RoundTrip.Run(target);
        Assert.Equal("Value2", result.Property?[2]);
    }

    public class ClassWithIntStringDictionary
    {
        [Encrypt]
        public Dictionary<int, string>? Property { get; set; }
    }

    [Fact]
    public async Task IntGuidDictionary()
    {
        var target = new ClassWithIntGuidDictionary
        {
            Property = new()
            {
                {
                    1, new("45b14050-065c-4be7-8bb8-f3b46b8d94e6")
                },
                {
                    2, new("74b69ad1-f9e8-4549-8524-cce4a8b4c38b")
                }
            }
        };
        var result = await RoundTrip.Run(target);
        Assert.Equal("74b69ad1-f9e8-4549-8524-cce4a8b4c38b", result.Property?[2].ToString());
    }

    public class ClassWithIntGuidDictionary
    {
        [Encrypt]
        public Dictionary<int, Guid>? Property { get; set; }
    }
}