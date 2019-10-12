using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class WrappedNodeConverter :
    JsonConverter
{
    Encrypter encrypter;
    JsonConverter innerConverter;

    public WrappedNodeConverter(Encrypter encrypter, JsonConverter innerConverter)
    {
        this.encrypter = encrypter;
        this.innerConverter = innerConverter;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var target = innerConverter.Serialize(value, serializer);
        var encrypted = encrypter.Encrypt(target);
        writer.WriteValue(encrypted);
    }

    public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
    {
        var value = (string) reader.Value;
        var decrypted = encrypter.Decrypt(value);
        return innerConverter.Deserialize(type, serializer, decrypted, existingValue);
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}