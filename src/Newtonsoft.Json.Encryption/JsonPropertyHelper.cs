using Newtonsoft.Json.Encryption;
using Newtonsoft.Json.Serialization;

static class JsonPropertyHelper
{
    public static void Manipulate(
        MemberInfo member,
        Encrypter encrypter,
        JsonProperty property)
    {
        var containsEncryptAttribute = member.ContainsEncryptAttribute();
        var containsNodeEncryptAttribute = member.ContainsNodeEncryptAttribute();
        if (!containsEncryptAttribute && !containsNodeEncryptAttribute)
        {
            return;
        }
        if (containsEncryptAttribute && containsNodeEncryptAttribute)
        {
            throw new($"Cannot contain both {nameof(EncryptAttribute)} and {nameof(NodeEncryptAttribute)}.");
        }
        if (containsNodeEncryptAttribute)
        {
            if (property.Converter == null)
            {
                property.Converter = new NodeConverter(encrypter);
                return;
            }
            property.Converter = new WrappedNodeConverter(encrypter, property.Converter);
            return;
        }

        var memberType = member.GetUnderlyingType();

        if (memberType == typeof(string))
        {
            VerifyConverterIsNull(property, member);
            property.Converter = new StringConverter(encrypter);
            return;
        }
        if (memberType.IsStringDictionary())
        {
            VerifyItemConverterIsNull(property, member);
            property.ItemConverter = new StringConverter(encrypter);
            return;
        }
        if (memberType.IsStringEnumerable())
        {
            VerifyItemConverterIsNull(property, member);
            property.ItemConverter = new StringConverter(encrypter);
            return;
        }

        if (memberType == typeof(Guid))
        {
            VerifyConverterIsNull(property, member);
            property.Converter = new GuidConverter(encrypter);
            return;
        }
        if (memberType.IsGuidDictionary())
        {
            VerifyItemConverterIsNull(property, member);
            property.ItemConverter = new GuidConverter(encrypter);
            return;
        }
        if (memberType.IsGuidEnumerable())
        {
            VerifyItemConverterIsNull(property, member);
            property.ItemConverter = new GuidConverter(encrypter);
            return;
        }

        if (memberType == typeof(byte[]))
        {
            VerifyConverterIsNull(property, member);
            property.Converter = new ByteArrayConverter(encrypter);
            return;
        }
        if (memberType.IsByteArrayDictionary())
        {
            VerifyItemConverterIsNull(property, member);
            property.ItemConverter = new ByteArrayConverter(encrypter);
            return;
        }
        if (memberType.IsByteArrayEnumerable())
        {
            VerifyItemConverterIsNull(property, member);
            property.ItemConverter = new ByteArrayConverter(encrypter);
            return;
        }
        throw new("Expected a string, a IDictionary<T,string>, a IEnumerable<string>, a Guid, a IDictionary<T,Guid>, a IEnumerable<Guid>, a byte[], a IDictionary<T,byte[]>, or a IEnumerable<byte[]>.");
    }

    static void VerifyItemConverterIsNull(JsonProperty property, MemberInfo member)
    {
        if (property.ItemConverter != null)
        {
            throw new($"Expected JsonProperty.ItemConverter to be null. Property: {member.FriendlyName()}");
        }
    }

    static void VerifyConverterIsNull(JsonProperty property, MemberInfo member)
    {
        if (property.Converter != null)
        {
            throw new($"Expected JsonProperty.Converter to be null. Property: {member.FriendlyName()}");
        }
    }

    static bool ContainsEncryptAttribute(this MemberInfo member)
    {
        return member.GetCustomAttribute<EncryptAttribute>() != null;
    }

    static bool ContainsNodeEncryptAttribute(this MemberInfo member)
    {
        return member.GetCustomAttribute<NodeEncryptAttribute>() != null;
    }

    static string FriendlyName(this MemberInfo member)
    {
        return $"{member.DeclaringType.FullName}.{member.Name}";
    }
}