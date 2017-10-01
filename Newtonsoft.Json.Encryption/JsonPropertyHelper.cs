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
        if (member.ContainsEncryptAttribute())
        {
            return;
        }
        var memberType = member.GetUnderlyingType();
        if (memberType == typeof(string))
        {
            property.Converter = new StringItemConverter(encrypter);
            property.MemberConverter = new StringItemConverter(encrypter);
            return;
        }
        if (memberType.IsStringDictionary())
        {
            property.ItemConverter = new StringItemConverter(encrypter);
            return;
        }
        if (memberType.IsStringEnumerable())
        {
            property.ItemConverter = new StringItemConverter(encrypter);
            return;
        }
        if (memberType == typeof(byte[]))
        {
            property.Converter = new ByteArrayItemConverter(encrypter);
            property.MemberConverter = new ByteArrayItemConverter(encrypter);
            return;
        }
        if (memberType.IsByteArrayDictionary())
        {
            property.ItemConverter = new ByteArrayItemConverter(encrypter);
            return;
        }
        if (memberType.IsByteArrayEnumerable())
        {
            property.ItemConverter = new ByteArrayItemConverter(encrypter);
            return;
        }
        throw new Exception("Expected a string, a IDictionary<T,string>, a IEnumerable<string>, a byte[], a IDictionary<T,byte[]>, or a IEnumerable<byte[]>.");
    }

    static bool ContainsEncryptAttribute(this MemberInfo member)
    {
        return member.GetCustomAttribute<EncryptAttribute>() == null;
    }
}