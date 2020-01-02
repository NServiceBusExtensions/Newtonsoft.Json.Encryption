using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NServiceBus;
using NServiceBus.Newtonsoft.Encryption;

class Snippets
{
    void Usage(byte[] key)
    {
        #region NsbUsage
        var configuration = new EndpointConfiguration("NServiceBusSample");
        var serialization = configuration.UseSerialization<NewtonsoftSerializer>();
        var encryptionFactory = new EncryptionFactory();
        serialization.Settings(
            new JsonSerializerSettings
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
