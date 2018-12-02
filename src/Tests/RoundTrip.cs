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
}