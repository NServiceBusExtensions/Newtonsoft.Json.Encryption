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
            property.Converter = new StringConverter(encrypter);
            property.MemberConverter = new StringConverter(encrypter);
            return;
        }
        if (memberType.IsStringDictionary())
        {
            property.ItemConverter = new StringConverter(encrypter);
            return;
        }
        if (memberType.IsStringEnumerable())
        {
            property.ItemConverter = new StringConverter(encrypter);
            return;
        }

        if (memberType == typeof(Guid))
        {
            property.Converter = new GuidConverter(encrypter);
            property.MemberConverter = new GuidConverter(encrypter);
            return;
        }
        if (memberType.IsGuidDictionary())
        {
            property.ItemConverter = new GuidConverter(encrypter);
            return;
        }
        if (memberType.IsGuidEnumerable())
        {
            property.ItemConverter = new GuidConverter(encrypter);
            return;
        }

        if (memberType == typeof(byte[]))
        {
            property.Converter = new ByteArrayConverter(encrypter);
            property.MemberConverter = new ByteArrayConverter(encrypter);
            return;
        }
        if (memberType.IsByteArrayDictionary())
        {
            property.ItemConverter = new ByteArrayConverter(encrypter);
            return;
        }
        if (memberType.IsByteArrayEnumerable())
        {
            property.ItemConverter = new ByteArrayConverter(encrypter);
            return;
        }
        throw new Exception("Expected a string, a IDictionary<T,string>, a IEnumerable<string>, a Guid, a IDictionary<T,Guid>, a IEnumerable<Guid>, a byte[], a IDictionary<T,byte[]>, or a IEnumerable<byte[]>.");
    }

    static bool ContainsEncryptAttribute(this MemberInfo member)
    {
        return member.GetCustomAttribute<EncryptAttribute>() == null;
    }
}