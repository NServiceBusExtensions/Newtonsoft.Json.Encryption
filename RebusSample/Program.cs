using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Newtonsoft.Encryption;
using Rebus.Serialization.Json;
using Rebus.Transport.FileSystem;

class Program
{
    static async Task Main()
    {
        var directory = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;

        using (var activator = new BuiltinHandlerActivator())
        {
            activator.Register(() => new Handler());
            var configurer = Configure.With(activator);

            configurer.Transport(t =>
            {
                t.UseFileSystem(directory, "RebusSample");
            });

            IBus bus = null;
            EncryptionFactory encryptionFactory = null;
            try
            {
                encryptionFactory = new EncryptionFactory();
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = encryptionFactory.GetContractResolver()
                };
                configurer.Serialization(s => { s.UseNewtonsoftJson(settings); });
                var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
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

                bus = configurer.Start();
                Console.WriteLine("Press any key to exit");

                await SendMessage(bus)
                    .ConfigureAwait(false);
                Console.ReadKey();
            }
            finally
            {
                bus?.Dispose();
                encryptionFactory?.Dispose();
            }
        }
    }

    static Task SendMessage(IBus bus)
    {
        var message = new MessageWithSecretData
        {
            Secret = "betcha can't guess my secret",
            SubProperty = new MySecretSubProperty
            {
                Secret = "My sub secret"
            },
            CreditCards = new List<CreditCardDetails>
            {
                new CreditCardDetails
                {
                    ValidTo = DateTime.UtcNow.AddYears(1),
                    Number = "312312312312312"
                },
                new CreditCardDetails
                {
                    ValidTo = DateTime.UtcNow.AddYears(2),
                    Number = "543645546546456"
                }
            }
        };
        return bus.SendLocal(message);
    }
}