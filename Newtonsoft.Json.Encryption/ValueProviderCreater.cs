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
                jsonProperty.ItemConverter = new ItemConverter(stringEncrypt);
                return;
            }
            if (propertyType.IsStringEnumerable())
            {
                jsonProperty.ItemConverter = new ItemConverter(stringEncrypt);
                return;
            }
            throw new Exception("Expected a string");
        }
    }
}