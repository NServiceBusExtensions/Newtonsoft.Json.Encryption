using System.Threading.Tasks;
using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

public static class RoundTrip
{
    public static T Run<T>(T instance)
    {
        using(var factory = new EncryptionFactory())
        using (var algorithm = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = factory.GetContractResolver()
            };

            string result;
            using (factory.GetEncryptSession(algorithm))
            {
                result = serializer.Serialize(instance);
            }

            Approvals.Verify(result);
            using (factory.GetDecryptSession(algorithm))
            {
                return serializer.Deserialize<T>(result);
            }
        }
    }

    public static async Task<T> RunAsync<T>(T instance)
    {
        using (var factory = new EncryptionFactory())
        using (var algorithm = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = factory.GetContractResolver()
            };

            string result;
            using (factory.GetEncryptSession(algorithm))
            {
                await Task.Delay(1).ConfigureAwait(false);
                result = serializer.Serialize(instance);
            }

            Approvals.Verify(result);
            using (factory.GetDecryptSession(algorithm))
            {
                await Task.Delay(1).ConfigureAwait(false);
                return serializer.Deserialize<T>(result);
            }
        }
    }
}