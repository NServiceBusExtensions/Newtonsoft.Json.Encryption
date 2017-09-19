using System;

namespace Newtonsoft.Json.Encryption
{
    public class ByteArrayItemConverter : JsonConverter
    {
        Encrypter encrypter;

        public ByteArrayItemConverter(Encrypter encrypter)
        {
            this.encrypter = encrypter;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(Convert.ToBase64String(encrypter.EncryptBytes((byte[]) value)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var fromBase64String = Convert.FromBase64String((string) reader.Value);
            return encrypter.DecryptBytes(fromBase64String);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}