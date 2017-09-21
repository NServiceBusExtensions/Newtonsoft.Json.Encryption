using System.Security.Cryptography;
using System.Text;
using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NUnit.Framework;
using ObjectApproval;

[TestFixture]
public class ThreadLocalSessionTest
{
    [Test]
    public void ExampleUsage()
    {
        // per system (rotated)
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");

        // per app domain
        var serializer = new JsonSerializer
        {
            ContractResolver = new ContractResolver(
                encrypter: new Encrypter(
                    encryptProvider: ThreadLocalSession.GetEncryptProvider(),
                    decryptProvider: ThreadLocalSession.GetDecryptProvider(),
                    encryptCleanup: ThreadLocalSession.GetEncryptCleanup(),
                    decryptCleanup: ThreadLocalSession.GetDecryptCleanup())
            )
        };

        // transferred as meta data with the serialized payload
        byte[] initVector;

        string serialized;

        // per serialize session
        using (var algorithm = new RijndaelManaged
        {
            Key = key
        })
        {
            initVector = algorithm.IV;
            using (new ThreadLocalSession(algorithm))
            {
                var instance = new ClassToSerialize
                {
                    Property1 = "Property1Value",
                    Property2 = "Property2Value"
                };
                serialized = serializer.Serialize(instance);
            }
        }

        // per deserialize session
        using (var algorithm = new RijndaelManaged
        {
            IV = initVector,
            Key = key
        })
        {
            using (new ThreadLocalSession(algorithm))
            {
                var deserialized = serializer.Deserialize<ClassToSerialize>(serialized);
                ObjectApprover.VerifyWithJson(deserialized);
            }
        }
    }

    [Test]
    public void Simple()
    {
        var serializer = BuildJsonSerializer();
        using (var algorithm = CryptoBuilder.Build())
        using (new ThreadLocalSession(algorithm))
        {
            var instance = new ClassToSerialize
            {
                Property1 = "Property1Value",
                Property2 = "Property2Value"
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
                Property1 = "Property1Value",
                Property2 = "Property2Value"
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
                encrypter: new Encrypter(
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
        public string Property1 { get; set; }
        [Encrypt]
        public string Property2 { get; set; }
    }

}