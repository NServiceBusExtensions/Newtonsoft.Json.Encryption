using System;

namespace Newtonsoft.Json.Encryption
{
    public class ByteArrayItemConverter : JsonConverter
    {
        StringEncrypt stringEncrypt;

        public ByteArrayItemConverter(StringEncrypt stringEncrypt)
        {
            this.stringEncrypt = stringEncrypt;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(stringEncrypt.Encrypt((byte[]) value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return stringEncrypt.Decrypt((byte[]) reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}