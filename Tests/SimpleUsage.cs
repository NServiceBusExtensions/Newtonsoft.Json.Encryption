using System.Collections.Generic;
using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class SimpleUsage
{
    [Test]
    public void RoundTripStringProperty()
    {
        var target = new ClassToEncrypt {Property = "Foo"};
        var result = RoundTrip(target);
        Assert.AreEqual("Foo", result.Property);
    }

    public class ClassToEncrypt
    {
        [Encrypt]
        public string Property { get; set; }
    }

    [Test]
    public void RoundTripDictionaryProperty()
    {
        var target = new ClassWithDictionary
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
        var result = RoundTrip(target);
        Assert.AreEqual("Value2", result.Property["Key2"]);
    }

    public class ClassWithDictionary
    {
        [Encrypt]
        public Dictionary<string, string> Property { get; set; }
    }

    [Test]
    public void RoundTripListProperty()
    {
        var target = new ClassWithList
        {
            Property = new List<string>
            {
                "Value1",
                "Value2"
            }
        };
        var result = RoundTrip(target);
        Assert.AreEqual("Value2", result.Property[1]);
    }

    public class ClassWithList
    {
        [Encrypt]
        public List<string> Property { get; set; }
    }

    public T RoundTrip<T>(T instance)
    {
        using (var crypto = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new EncryptionContractResolver(
                    stringEncrypt: new StringEncrypt(
                        encryptProvider: () => crypto.CreateEncryptor(),
                        decryptProvider: () => crypto.CreateDecryptor(),
                        cryptoCleanup: transform => { })
                )
            };

            var result = serializer.Serialize(instance);

            Approvals.Verify(result);
            return serializer.Deserialize<T>(result);
        }
    }
}
