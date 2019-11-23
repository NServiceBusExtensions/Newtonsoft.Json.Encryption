using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using VerifyXunit;

public static class RoundTrip
{
    public static async Task<T> Run<T>(this VerifyBase verifyBase,T instance)
    {
        using var factory = new EncryptionFactory();
        using var algorithm = CryptoBuilder.Build();
        var serializer = new JsonSerializer
        {
            ContractResolver = factory.GetContractResolver()
        };

        string result;
        using (factory.GetEncryptSession(algorithm))
        {
            result = serializer.Serialize(instance);
        }

        await verifyBase.Verify(result);
        using (factory.GetDecryptSession(algorithm))
        {
            return serializer.Deserialize<T>(result);
        }
    }
}