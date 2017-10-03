using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class GuidConverter : JsonConverter
{
    Encrypter encrypter;

    public GuidConverter(Encrypter encrypter)
    {
        this.encrypter = encrypter;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var guid = (Guid) value;
        var base64String = encrypter.EncryptGuidToString(guid);
        writer.WriteValue(base64String);
    }


    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var value = (string)reader.Value;
        return encrypter.DecryptGuidFromString(value);
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}