using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NServiceBus;
using NServiceBus.Newtonsoft.Encryption;

class Program
{
    static async Task Main()
    {
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        Console.Title = "NServiceBusSample";

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

        configuration.UsePersistence<LearningPersistence>();
        configuration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(configuration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");

        await SendMessage(endpointInstance)
            .ConfigureAwait(false);

        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        encryptionFactory.Dispose();
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
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
        return endpointInstance.SendLocal(message);
    }
}