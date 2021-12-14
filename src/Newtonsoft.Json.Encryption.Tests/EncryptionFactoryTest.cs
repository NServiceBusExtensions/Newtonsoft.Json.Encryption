﻿using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

[UsesVerify]
public class EncryptionFactoryTest
{
    [Fact]
    public async Task ExampleUsage()
    {
        // per system (periodically rotated)
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");

        // per app domain
        using var factory = new EncryptionFactory();
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
                    Property1 = "Property1Value",
                    Property2 = "Property2Value"
                };
                serialized = serializer.Serialize(instance);
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
            {
                var deserialized = serializer.Deserialize<ClassToSerialize>(serialized);
                await Verify(deserialized);
            }
        }
    }

    [Fact]
    public async Task Simple()
    {
        var factory = new EncryptionFactory();
        var serializer = new JsonSerializer
        {
            ContractResolver = factory.GetContractResolver()
        };
        using var algorithm = CryptoBuilder.Build();
        using (factory.GetEncryptSession(algorithm))
        {
            var instance = new ClassToSerialize
            {
                Property1 = "Property1Value",
                Property2 = "Property2Value"
            };
            var result = serializer.Serialize(instance);
            await Verify(result);
        }
    }

    [Fact]
    public async Task RoundTrip()
    {
        var factory = new EncryptionFactory();
        var serializer = new JsonSerializer
        {
            ContractResolver = factory.GetContractResolver()
        };
        using var algorithm = CryptoBuilder.Build();
        string serialized;
        using (factory.GetEncryptSession(algorithm))
        {
            var instance = new ClassToSerialize
            {
                Property1 = "Property1Value",
                Property2 = "Property2Value"
            };
            serialized = serializer.Serialize(instance);
        }
        using (factory.GetDecryptSession(algorithm))
        {
            var result = serializer.Deserialize<ClassToSerialize>(serialized);
            await Verify(result);
        }
    }

    public class ClassToSerialize
    {
        [Encrypt]
        public string? Property1 { get; set; }

        [Encrypt]
        public string? Property2 { get; set; }
    }
}