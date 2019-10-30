using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;

static class Extensions
{

    public static Guid DecryptGuidFromString(this Encrypter encrypter, string value)
    {
        var fromBase64String = Convert.FromBase64String(value);
        var decryptBytes = encrypter.DecryptBytes(fromBase64String);
        return new Guid(decryptBytes);
    }

    public static string EncryptGuidToString(this Encrypter encrypter, Guid guid)
    {
        var byteArray = guid.ToByteArray();
        var encryptBytes = encrypter.EncryptBytes(byteArray);
        return Convert.ToBase64String(encryptBytes);
    }

    public static byte[] DecryptBytesFromString(this Encrypter encrypter, string value)
    {
        var fromBase64String = Convert.FromBase64String(value);
        return encrypter.DecryptBytes(fromBase64String);
    }

    public static string EncryptBytesToString(this Encrypter encrypter, byte[] bytes)
    {
        var encryptBytes = encrypter.EncryptBytes(bytes);
        return Convert.ToBase64String(encryptBytes);
    }

    public static string Serialize(this JsonConverter converter, object value, JsonSerializer serializer)
    {
        var builder = new StringBuilder();
        using (var stringWriter = new StringWriter(builder))
        {
            using var textWriter = new JsonTextWriter(stringWriter);
            converter.WriteJson(textWriter, value, serializer);
        }

        return builder.ToString();
    }

    public static object Deserialize(this JsonConverter converter, Type type, JsonSerializer serializer, string decrypted, object existingValue)
    {
        using var stringReader = new StringReader(decrypted);
        using var textReader = new JsonTextReader(stringReader);
        return converter.ReadJson(textReader, type, existingValue, serializer);
    }

    public static string? Serialize(this JsonSerializer serializer, object value)
    {
        var builder = new StringBuilder();
        using (var writer = new StringWriter(builder))
        {
            serializer.Serialize(writer, value);
        }
        return builder.ToString();
    }

    public static object Deserialize(this JsonSerializer serializer, Type type, string? value)
    {
        using var reader = new StringReader(value);
        return serializer.Deserialize(reader, type);
    }

    public static Type GetUnderlyingType(this MemberInfo member)
    {
        if (member is FieldInfo field)
        {
            return field.FieldType;
        }
        if (member is PropertyInfo property)
        {
            return property.PropertyType;
        }
        throw new ArgumentException
        (
            "Input MemberInfo must be if type FieldInfo or PropertyInfo"
        );
    }

    public static object GetValue(this MemberInfo member, object instance)
    {
        if (member is FieldInfo fieldInfo)
        {
            return fieldInfo.GetValue(instance);
        }
        if (member is PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(instance, null);
        }
        throw new ArgumentException
        (
            "Input MemberInfo must be if type FieldInfo or PropertyInfo"
        );
    }

    public static void SetValue(this MemberInfo member, object instance, object value)
    {
        if (member is FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(instance, value);
            return;
        }
        if (member is PropertyInfo propertyInfo)
        {
            propertyInfo.SetValue(instance, value);
            return;
        }
        throw new ArgumentException
        (
            "Input MemberInfo must be if type FieldInfo or PropertyInfo"
        );
    }

    static TypeInfo stringEnumerable = typeof(IEnumerable<string>).GetTypeInfo();
    static TypeInfo guidEnumerable = typeof(IEnumerable<Guid>).GetTypeInfo();
    static TypeInfo byteArrayEnumerable = typeof(IEnumerable<byte[]>).GetTypeInfo();

    public static bool IsStringDictionary(this Type type)
    {
        return IsDictionary(type, typeof(string));
    }

    public static bool IsGuidDictionary(this Type type)
    {
        return IsDictionary(type, typeof(Guid));
    }

    public static bool IsByteArrayDictionary(this Type type)
    {
        return IsDictionary(type, typeof(byte[]));
    }

    static bool IsDictionary(Type type, Type valueTypeToCheck)
    {
        var typeInfo = type.GetTypeInfo();
        return typeInfo.ImplementedInterfaces
            .Any(x => IsDictionaryInterface(x, valueTypeToCheck));
    }

    static bool IsDictionaryInterface(Type type, Type valueTypeToCheck)
    {
        var typeInfo = type.GetTypeInfo();
        var isDict = typeInfo.IsGenericType &&
                     typeInfo.GetGenericTypeDefinition() == typeof(IDictionary<,>);
        if (!isDict)
        {
            return false;
        }
        var valueType = typeInfo.GenericTypeArguments[1];
        return valueType == valueTypeToCheck;
    }

    public static bool IsStringEnumerable(this Type type)
    {
        return stringEnumerable.IsAssignableFrom(type.GetTypeInfo());
    }

    public static bool IsGuidEnumerable(this Type type)
    {
        return guidEnumerable.IsAssignableFrom(type.GetTypeInfo());
    }

    public static bool IsByteArrayEnumerable(this Type type)
    {
        return byteArrayEnumerable.IsAssignableFrom(type.GetTypeInfo());
    }
}