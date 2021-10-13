using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Newtonsoft.Encryption;
using Rebus.Serialization.Json;
// ReSharper disable UnusedParameter.Local

class Snippets
{
    void Usage(byte[] key)
    {
        #region RebugsUsage
        var activator = new BuiltinHandlerActivator();

        activator.Register(() => new Handler());
        var configurer = Configure.With(activator);

        var encryptionFactory = new EncryptionFactory();
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = encryptionFactory.GetContractResolver()
        };
        configurer.Serialization(s => { s.UseNewtonsoftJson(settings); });
        configurer.EnableJsonEncryption(
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

internal class Handler: IHandleMessages
{
}
