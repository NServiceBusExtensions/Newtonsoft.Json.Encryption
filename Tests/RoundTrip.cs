using ApprovalTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

public static class RoundTrip
{

    public static T Run<T>(T instance)
    {
        using (var crypto = CryptoBuilder.Build())
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new ContractResolver(
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