using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;
using ObjectApproval;

[TestFixture]
public class ThreadLocalSessionTest
{
    [Test]
    public void Simple()
    {
        var serializer = BuildJsonSerializer();
        using (var algorithm = CryptoBuilder.Build())
        using (new ThreadLocalSession(algorithm))
        {
            var instance = new ClassToSerialize
            {
                Property = "A"
            };
            var result = serializer.Serialize(instance);
            Approvals.Verify(result);
        }
    }

    [Test]
    public void RoundTrip()
    {
        var serializer = BuildJsonSerializer();
        using (var algorithm = CryptoBuilder.Build())
        using (new ThreadLocalSession(algorithm))
        {
            var instance = new ClassToSerialize
            {
                Property = "A"
            };
            var serialized = serializer.Serialize(instance);
            var result = serializer.Deserialize<ClassToSerialize>(serialized);
            ObjectApprover.VerifyWithJson(result);
        }
    }

    static JsonSerializer BuildJsonSerializer()
    {
        return new JsonSerializer
        {
            ContractResolver = new ContractResolver(
                stringEncrypt: new StringEncrypt(
                    encryptProvider: ThreadLocalSession.GetEncryptProvider(),
                    decryptProvider: ThreadLocalSession.GetDecryptProvider(),
                    encryptCleanup: ThreadLocalSession.GetEncryptCleanup(),
                    decryptCleanup: ThreadLocalSession.GetDecryptCleanup())
            )
        };
    }

    public class ClassToSerialize
    {
        [Encrypt]
        public string Property { get; set; }
    }

}