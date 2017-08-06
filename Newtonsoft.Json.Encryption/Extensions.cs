using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

static class Extensions
{
    static TypeInfo stringEnumerable = typeof(IEnumerable<string>).GetTypeInfo();

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

    public static bool IsStringDictionary(this Type type)
    {
        var typeInfo = type.GetTypeInfo();
        return typeInfo.ImplementedInterfaces
            .Any(implementedInterface => implementedInterface.IsStringDictionaryInterface());
    }

    public static bool IsStringDictionaryInterface(this Type type)
    {
        var typeInfo = type.GetTypeInfo();
        var isDict = typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IDictionary<,>);
        if (!isDict)
        {
            return false;
        }
        var valueType = typeInfo.GenericTypeArguments[1];
        return valueType == typeof(string);
    }

    public static bool IsStringEnumerable(this Type type)
    {
        return stringEnumerable.IsAssignableFrom(type.GetTypeInfo());
    }

}