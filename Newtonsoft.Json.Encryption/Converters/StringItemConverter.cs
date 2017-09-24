using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class StringItemConverter : JsonConverter
{
    Encrypter encrypter;

    public StringItemConverter(Encrypter encrypter)
    {
        this.encrypter = encrypter;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(encrypter.Encrypt((string) value));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return encrypter.Decrypt((string) reader.Value);
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}