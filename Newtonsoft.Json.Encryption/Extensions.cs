using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

static class Extensions
{
    public static Type GetUnderlyingType(this MemberInfo member)
    {
        if (member is FieldInfo info)
        {
            return info.FieldType;
        }
        if (member is PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
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
    static TypeInfo byteArrayEnumerable = typeof(IEnumerable<byte[]>).GetTypeInfo();

    public static bool IsStringDictionary(this Type type)
    {
        return IsDictionary(type, typeof(string));
    }
    public static bool IsByteArrayDictionary(this Type type)
    {
        return IsDictionary(type, typeof(byte[]));
    }

    private static bool IsDictionary(Type type, Type valueTypeToCheck)
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
    public static bool IsByteArrayEnumerable(this Type type)
    {
        return byteArrayEnumerable.IsAssignableFrom(type.GetTypeInfo());
    }

}