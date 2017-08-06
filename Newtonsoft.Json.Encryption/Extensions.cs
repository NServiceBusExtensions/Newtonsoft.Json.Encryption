using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

static class Extensions
{
    public static Type GetUnderlyingType(this MemberInfo member)
    {
        var info = member as FieldInfo;
        if (info != null)
        {
            return info.FieldType;
        }
        var propertyInfo = member as PropertyInfo;
        if (propertyInfo != null)
        {
            return propertyInfo.PropertyType;
        }
        throw new ArgumentException
        (
            "Input MemberInfo must be if type FieldInfo or PropertyInfo"
        );
    }

    public static bool IsStringValuedDictionary(this Type type)
    {
        var typeInfo = type.GetTypeInfo();
        return typeInfo.ImplementedInterfaces
            .Any(implementedInterface => implementedInterface.IsStringValuedDictionaryInterface());
    }

    public static bool IsStringValuedDictionaryInterface(this Type type)
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

    public static string GetValue(this MemberInfo member, object instance)
    {
        var fieldInfo = member as FieldInfo;
        if (fieldInfo != null)
        {
            return (string) fieldInfo.GetValue(instance);
        }
        var propertyInfo = member as PropertyInfo;
        if (propertyInfo != null)
        {
            return (string) propertyInfo.GetValue(instance, null);
        }
        throw new ArgumentException
        (
            "Input MemberInfo must be if type FieldInfo or PropertyInfo"
        );
    }
    public static void SetValue(this MemberInfo member, object instance, string value)
    {
        var fieldInfo = member as FieldInfo;
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(instance, value);
            return;
        }
        var propertyInfo = member as PropertyInfo;
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(instance, value);
            return;
        }
        throw new ArgumentException
        (
            "Input MemberInfo must be if type FieldInfo or PropertyInfo"
        );
    }
}