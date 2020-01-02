using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class Snippets
{
    void Workflow()
    {
        #region Workflow

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

        #region serialize
        // per serialize session
        using (var algorithm = new RijndaelManaged
        {
            Key = key
        })
        {
            //TODO: store initVector for use in deserialization
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
        #endregion

        #region deserialize
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
                Console.WriteLine(deserialized!.Property);
            }
        }

        #endregion
        #endregion
    }

    class ClassToSerialize
    {
        public string Property { get; set; } = null!;
    }
}
