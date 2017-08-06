using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;

[TestFixture]
public class SimpleUsage
{
    [Test]
    public void RoundTrip()
    {
        using (var crypto = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new EncryptionContractResolver(
                    encryptProvider: () => crypto.CreateEncryptor(),
                    decryptProvider: () => crypto.CreateDecryptor(),
                    cryptoCleanup: transform => { })
            };

            var target = new ClassToEncrypt {Property = "Foo"};
            var result = serializer.RoundTrip(target);

            Assert.AreEqual("Foo", result.Property);
        }
    }

    [Test]
    public void Encrypt()
    {
        using (var crypto = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new EncryptionContractResolver(
                    encryptProvider: () => crypto.CreateEncryptor(),
                    decryptProvider: () => crypto.CreateDecryptor(),
                    cryptoCleanup: transform => { })
            };

            var target = new ClassToEncrypt { Property = "Foo" };
            var result = serializer.Serialize(target);

            Approvals.Verify(result);
        }
    }


    public class ClassToEncrypt
    {
        [Encrypt]
        public string Property { get; set; }
    }
}