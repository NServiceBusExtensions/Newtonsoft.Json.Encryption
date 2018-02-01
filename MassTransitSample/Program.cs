using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Newtonsoft.Encryption;
using MassTransitSample;
using Newtonsoft.Json.Encryption;

class Program
{
    static async Task Main()
    {
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        var encryptionFactory = new EncryptionFactory();
        var busControl = Bus.Factory.CreateUsingInMemory(configurator =>
        {
            configurator.ConfigureJsonSerializer(settings =>
            {
                settings.ContractResolver = encryptionFactory.GetContractResolver();
                return settings;
            });

            configurator.EnableJsonEncryption(
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
        });

        var busHandle = await busControl.StartAsync();
        await SendMessage(busControl);
        Console.ReadKey();
        await busHandle.StopAsync();
    }

    static Task SendMessage(IBusControl busControl)
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
        return busControl.Publish(message);
    }
}