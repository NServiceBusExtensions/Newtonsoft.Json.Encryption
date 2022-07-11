using System.Security.Cryptography;
using Newtonsoft.Json.Encryption;
using NServiceBus;
using NServiceBus.Newtonsoft.Encryption;
// ReSharper disable UnusedParameter.Local

class Snippets
{
    void Usage(byte[] key)
    {
        #region NsbUsage
        var configuration = new EndpointConfiguration("NServiceBusSample");
        var serialization = configuration.UseSerialization<NewtonsoftJsonSerializer>();
        var encryptionFactory = new EncryptionFactory();
        serialization.Settings(
            new()
            {
                ContractResolver = encryptionFactory.GetContractResolver()
            });

        configuration.EnableJsonEncryption(
            encryptionFactory: encryptionFactory,
            encryptStateBuilder: () =>
            (
                algorithm: new RijndaelManaged
                {
                    Key = key
                },
                keyId: "1"
            ),
            decryptStateBuilder: (keyId, initVector) =>
                new RijndaelManaged
                {
                    Key = key,
                    IV = initVector
                });
        #endregion
    }

}
