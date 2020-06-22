using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using VerifyXunit;

public static class RoundTrip
{
    public static async Task<T> Run<T>(T instance, [CallerFilePath] string sourceFile = "")
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

        await Verifier.Verify(result,null,sourceFile);
        using (factory.GetDecryptSession(algorithm))
        {
            return serializer.Deserialize<T>(result);
        }
    }
}