using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public static class JsonPropertyHelper {

        public static void Manipulate(
            MemberInfo member,
            StringEncrypt stringEncrypt,
            JsonProperty jsonProperty)
        {
            if (member.GetCustomAttribute<EncryptAttribute>() == null)
            {
                return;
            }
            var propertyType = member.GetUnderlyingType();
            if (propertyType == typeof(string))
            {
                jsonProperty.ValueProvider = new StringValueProvider(
                    targetMember: member,
                    stringEncrypt: stringEncrypt);
                return;
            }
            if (propertyType.IsStringDictionary())
            {
                jsonProperty.ItemConverter = new StringItemConverter(stringEncrypt);
                return;
            }
            if (propertyType.IsStringEnumerable())
            {
                jsonProperty.ItemConverter = new StringItemConverter(stringEncrypt);
                return;
            }
            if (propertyType == typeof(byte[]))
            {
                jsonProperty.ValueProvider = new ByteArrayValueProvider(
                    targetMember: member,
                    stringEncrypt: stringEncrypt);
                return;
            }
            if (propertyType.IsByteArrayDictionary())
            {
                jsonProperty.ItemConverter = new ByteArrayItemConverter(stringEncrypt);
                return;
            }
            if (propertyType.IsByteArrayEnumerable())
            {
                jsonProperty.ItemConverter = new ByteArrayItemConverter(stringEncrypt);
                return;
            }
            throw new Exception("Expected a string, a IDictionary<T,string>, a IEnumerable<string>, a byte[], a IDictionary<T,byte[]>, or a IEnumerable<byte[]>.");
        }
    }
}