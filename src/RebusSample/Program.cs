using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Newtonsoft.Encryption;
using Rebus.Serialization.Json;
using Rebus.Transport.FileSystem;
// ReSharper disable UnusedParameter.Local
#pragma warning disable SYSLIB0022

class Program
{
    static async Task Main()
    {
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        Console.Title = "RebusSample";
        var directory = Directory.GetParent(Assembly.GetEntryAssembly()?.Location!)!.FullName;

        var activator = new BuiltinHandlerActivator();

        activator.Register(() => new Handler());
        var configurer = Configure.With(activator);

        var encryptionFactory = new EncryptionFactory();
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = encryptionFactory.GetContractResolver()
        };
        configurer.Serialization(s =>
        {
            s.UseNewtonsoftJson(settings);
        });
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

        configurer.Transport(t =>
        {
            t.UseFileSystem(directory, "RebusSample");
        });

        var bus = configurer.Start();
        Console.WriteLine("Press any key to exit");

        await SendMessage(bus)
            ;
        Console.ReadKey();
        bus?.Dispose();
        encryptionFactory.Dispose();
        activator.Dispose();
    }


    static Task SendMessage(IBus bus)
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
        return bus.SendLocal(message);
    }
}