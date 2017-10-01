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
        var byteArray = guid.ToByteArray();
        var encryptBytes = encrypter.EncryptBytes(byteArray);
        var base64String = Convert.ToBase64String(encryptBytes);
        writer.WriteValue(base64String);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var value = (string)reader.Value;
        var fromBase64String = Convert.FromBase64String(value);
        var decryptBytes = encrypter.DecryptBytes(fromBase64String);
        return new Guid(decryptBytes);
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}