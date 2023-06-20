using System.Security.Cryptography;
using Newtonsoft.Json.Encryption;
using NServiceBus.Newtonsoft.Encryption;
// ReSharper disable UnusedParameter.Local
#pragma warning disable SYSLIB0022

class Program
{
    static async Task Main()
    {
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        Console.Title = "NServiceBusSample";

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

        configuration.UsePersistence<LearningPersistence>();
        configuration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(configuration)
            ;
        Console.WriteLine("Press any key to exit");

        await SendMessage(endpointInstance)
            ;

        Console.ReadKey();
        await endpointInstance.Stop()
            ;
        encryptionFactory.Dispose();
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        var message = new MessageWithSecretData
        {
            Secret = "betcha can't guess my secret",
            SubProperty = new()
            {
                Secret = "My sub secret"
            },
            CreditCards = new()
            {
                new()
                {
                    ValidTo = DateTime.UtcNow.AddYears(1),
                    Number = "312312312312312"
                },
                new()
                {
                    ValidTo = DateTime.UtcNow.AddYears(2),
                    Number = "543645546546456"
                }
            }
        };
        return endpointInstance.SendLocal(message);
    }
}