using System;
using System.Reflection;
using Newtonsoft.Json.Encryption;
using Newtonsoft.Json.Serialization;

static class JsonPropertyHelper
{
    public static void Manipulate(
        MemberInfo member,
        Encrypter encrypter,
        JsonProperty property)
    {
        if (member.GetCustomAttribute<EncryptAttribute>() == null)
        {
            return;
        }
        var propertyType = member.GetUnderlyingType();
        if (propertyType == typeof(string))
        {
            property.ValueProvider = new StringValueProvider(member, encrypter);
            return;
        }
        if (propertyType.IsStringDictionary())
        {
            property.ItemConverter = new StringItemConverter(encrypter);
            return;
        }
        if (propertyType.IsStringEnumerable())
        {
            property.ItemConverter = new StringItemConverter(encrypter);
            return;
        }
        if (propertyType == typeof(byte[]))
        {
            property.ValueProvider = new ByteArrayValueProvider(member, encrypter);
            return;
        }
        if (propertyType.IsByteArrayDictionary())
        {
            property.ItemConverter = new ByteArrayItemConverter(encrypter);
            return;
        }
        if (propertyType.IsByteArrayEnumerable())
        {
            property.ItemConverter = new ByteArrayItemConverter(encrypter);
            return;
        }
        throw new Exception("Expected a string, a IDictionary<T,string>, a IEnumerable<string>, a byte[], a IDictionary<T,byte[]>, or a IEnumerable<byte[]>.");
    }
}