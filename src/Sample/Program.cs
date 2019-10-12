using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class Program
{
    static void Main()
    {
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
                {
                    using var stringReader = new StringReader(serialized);
                    using var jsonReader = new JsonTextReader(stringReader);
                    var deserialized = serializer.Deserialize<ClassToSerialize>(jsonReader);
                    Console.WriteLine(deserialized.Property);
                }
            }
        }
        Console.ReadKey();
    }
}