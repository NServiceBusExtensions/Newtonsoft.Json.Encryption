# Newtonsoft.Json.Encryption

## The nuget package [![NuGet Status](http://img.shields.io/nuget/v/Newtonsoft.Json.Encryption.svg?style=flat)](https://www.nuget.org/packages/Newtonsoft.Json.Encryption/)

https://nuget.org/packages/Newtonsoft.Json.Encryption/

    PM> Install-Package Newtonsoft.Json.Encryption


### Decorating properties

```
public class ClassToSerialize
{
    [Encrypt]
    public string Property { get; set; }
}
```


### Supported property types

 * `string`
 * `byte[]`
 * `IDictionary<T, string>`
 * `IDictionary<T, byte[]>`
 * `IEnumerable<string>`
 * `IEnumerable<byte[]>`

Note that only the values in a `IDictionary` are encrypted.


### Usage

```
// per system (periodically rotated)
var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");

// per app domain
using (var factory = new EncryptionFactory())
{
    var serializer = new JsonSerializer
    {
        ContractResolver = factory.GetContractResolver()
    };

    // transferred as meta data with the serialized payload
    byte[] initVector;

    string serialized;

    // per serialize session
    using (var algorithm = new RijndaelManaged
    {
        Key = key
    })
    {
        initVector = algorithm.IV;
        using (factory.GetEncryptSession(algorithm))
        {
            var instance = new ClassToSerialize
            {
                Property = "PropertyValue",
            };
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, instance);
            }
            serialized = builder.ToString();
        }
    }

    // per deserialize session
    using (var algorithm = new RijndaelManaged
    {
        IV = initVector,
        Key = key
    })
    {
        using (factory.GetDecryptSession(algorithm))
        using (var stringReader = new StringReader(serialized))
        using (var jsonReader = new JsonTextReader(stringReader))
        {
            var deserialized = serializer.Deserialize<ClassToSerialize>(jsonReader);
            Console.WriteLine(deserialized.Property);
        }
    }
}
```

## Rebus


### The nuget package [![NuGet Status](http://img.shields.io/nuget/v/Rebus.Newtonsoft.Encryption.svg?style=flat)](https://www.nuget.org/packages/Rebus.Newtonsoft.Encryption/)

https://nuget.org/packages/Rebus.Newtonsoft.Encryption/

    PM> Install-Package Rebus.Newtonsoft.Encryption


### Usage

```
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
```


## NServiceBus


### The nuget package [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Newtonsoft.Encryption.svg?style=flat)](https://www.nuget.org/packages/NServiceBus.Newtonsoft.Encryption/)

https://nuget.org/packages/NServiceBus.Newtonsoft.Encryption/

    PM> Install-Package NServiceBus.Newtonsoft.Encryption


### Usage

```
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
```