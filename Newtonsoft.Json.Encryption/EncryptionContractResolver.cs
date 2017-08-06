using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionContractResolver : DefaultContractResolver
    {
        StringEncrypt stringEncrypt;

        public EncryptionContractResolver(StringEncrypt stringEncrypt)
        {
            this.stringEncrypt = stringEncrypt;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty == null)
            {
                return null;
            }
            if (ContractBuilder.TryCreate(member, stringEncrypt,out var converter))
            {
                jsonProperty.Converter = converter;
            }

            return jsonProperty;
        }



    }

    public static class ContractBuilder
    {

        public static bool TryCreate(MemberInfo member, StringEncrypt stringEncrypt, out JsonConverter converter)
        {
            if (member.GetCustomAttribute<EncryptAttribute>() == null)
            {
                converter = null;
                return false;
            }
            var underlyingType = member.GetUnderlyingType();

            if (underlyingType == typeof(string))
            {
                converter = new StringEncryptionConverter(stringEncrypt);
                return true;
            }

            if (underlyingType.IsStringDictionary() || underlyingType.IsStringEnumerable())
            {
                converter = new StringEncryptionConverter(stringEncrypt);
                return true;
            }

            converter = null;
            return false;
        }
    }
}