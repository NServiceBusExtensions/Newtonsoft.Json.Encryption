using System;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionConverter : JsonConverter
    {
        StringEncrypt stringEncrypt;

        public EncryptionConverter(StringEncrypt stringEncrypt)
        {
            this.stringEncrypt = stringEncrypt;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(stringEncrypt.Encrypt((string) value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return stringEncrypt.Decrypt((string) reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}