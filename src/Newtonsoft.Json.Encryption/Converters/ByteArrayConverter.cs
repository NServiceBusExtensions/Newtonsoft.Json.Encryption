using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class ByteArrayConverter :
    JsonConverter
{
    Encrypter encrypter;

    public ByteArrayConverter(Encrypter encrypter) =>
        this.encrypter = encrypter;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var bytes = (byte[]) value!;
        var base64String = encrypter.EncryptBytesToString(bytes);
        writer.WriteValue(base64String);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var value = (string) reader.Value!;
        return encrypter.DecryptBytesFromString(value);
    }

    public override bool CanConvert(Type objectType) =>
        throw new NotImplementedException();
}