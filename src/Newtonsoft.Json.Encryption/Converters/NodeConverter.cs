using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class NodeConverter :
    JsonConverter
{
    Encrypter encrypter;

    public NodeConverter(Encrypter encrypter)
    {
        this.encrypter = encrypter;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var target = serializer.Serialize(value!);
        var encrypted = encrypter.Encrypt(target);
        writer.WriteValue(encrypted);
    }

    public override object? ReadJson(JsonReader reader, Type type, object? existingValue, JsonSerializer serializer)
    {
        var value = (string) reader.Value!;
        var decrypted = encrypter.Decrypt(value);
        return serializer.Deserialize(type, decrypted);
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}