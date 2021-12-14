using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

public class AsyncUsage
{
    [Fact]
    public async Task Works()
    {
        var target = new ClassWithString
        {
            Property = "Foo"
        };

        using var factory = new EncryptionFactory();
        using var algorithm = CryptoBuilder.Build();
        var serializer = new JsonSerializer
        {
            ContractResolver = factory.GetContractResolver()
        };

        string serialized;
        using (factory.GetEncryptSession(algorithm))
        {
            await Task.Delay(1);
            serialized = serializer.Serialize(target);
        }

        using (factory.GetDecryptSession(algorithm))
        {
            await Task.Delay(1);
            var result = serializer.Deserialize<ClassWithString>(serialized);
            Assert.Equal("Foo", result.Property);
        }
    }

    public class ClassWithString
    {
        [Encrypt]
        public string? Property { get; set; }
    }
}