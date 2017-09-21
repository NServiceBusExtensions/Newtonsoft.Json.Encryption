using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

public static class RoundTrip
{
    public static T Run<T>(T instance)
    {
        using (var algorithm = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new ContractResolver(
                    encrypter: new Encrypter(
                        encryptProvider: () => algorithm.CreateEncryptor(),
                        decryptProvider: () => algorithm.CreateDecryptor(),
                        encryptCleanup: transform => { transform.Dispose(); },
                        decryptCleanup: transform => { transform.Dispose(); })
                )
            };

            var result = serializer.Serialize(instance);

            Approvals.Verify(result);
            return serializer.Deserialize<T>(result);
        }
    }
}