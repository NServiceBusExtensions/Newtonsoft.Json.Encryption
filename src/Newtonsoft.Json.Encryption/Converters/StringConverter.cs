using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

class StringConverter :
    JsonConverter
{
    Encrypter encrypter;

    public StringConverter(Encrypter encrypter) =>
        this.encrypter = encrypter;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var encrypted = encrypter.Encrypt((string?) value);
        writer.WriteValue(encrypted);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var value = (string) reader.Value!;
        return encrypter.Decrypt(value);
    }

    public override bool CanConvert(Type objectType) =>
        throw new NotImplementedException();
}