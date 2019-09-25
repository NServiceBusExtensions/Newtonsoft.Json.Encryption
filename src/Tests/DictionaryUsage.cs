using System;
using System.Collections.Generic;
using Newtonsoft.Json.Encryption;
using Xunit;
using Xunit.Abstractions;

public class DictionaryUsageUsage: TestBase
{
    public DictionaryUsageUsage(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
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
        Assert.Equal(new byte[] {5, 6}, result.Property?["Key2"]);
    }

    public class ClassWithByteArrayDictionary
    {
        [Encrypt]
        public Dictionary<string, byte[]>? Property { get; set; }
    }

    [Fact]
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
        Assert.Equal("Value2", result.Property?["Key2"]);
    }

    public class ClassWithStringDictionary
    {
        [Encrypt]
        public Dictionary<string, string>? Property { get; set; }
    }

    [Fact]
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
        Assert.Equal("Value2", result.Property?[2]);
    }

    public class ClassWithIntStringDictionary
    {
        [Encrypt]
        public Dictionary<int, string>? Property { get; set; }
    }

    [Fact]
    public void IntGuidDictionary()
    {
        var target = new ClassWithIntGuidDictionary
        {
            Property = new Dictionary<int, Guid>
            {
                {
                    1, new Guid("45b14050-065c-4be7-8bb8-f3b46b8d94e6")
                },
                {
                    2, new Guid("74b69ad1-f9e8-4549-8524-cce4a8b4c38b")
                }
            }
        };
        var result = RoundTrip.Run(target);
        Assert.Equal("74b69ad1-f9e8-4549-8524-cce4a8b4c38b", result.Property?[2].ToString());
    }

    public class ClassWithIntGuidDictionary
    {
        [Encrypt]
        public Dictionary<int, Guid>? Property { get; set; }
    }
}