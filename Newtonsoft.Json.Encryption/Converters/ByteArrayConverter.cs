using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class ByteArrayConverter : JsonConverter
{
    Encrypter encrypter;

    public ByteArrayConverter(Encrypter encrypter)
    {
        this.encrypter = encrypter;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var bytes = (byte[]) value;
        var encryptBytes = encrypter.EncryptBytes(bytes);
        var base64String = Convert.ToBase64String(encryptBytes);
        writer.WriteValue(base64String);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var value = (string) reader.Value;
        var fromBase64String = Convert.FromBase64String(value);
        return encrypter.DecryptBytes(fromBase64String);
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}