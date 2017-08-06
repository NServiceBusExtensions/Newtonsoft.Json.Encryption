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

            if (member.GetCustomAttribute<EncryptAttribute>() == null)
            {
                return jsonProperty;
            }
            var underlyingType = member.GetUnderlyingType();

            if (underlyingType == typeof(string))
            {
                jsonProperty.Converter = new EncryptionConverter(stringEncrypt);
                return jsonProperty;
            }

            if (underlyingType.IsStringValuedDictionary())
            {
                jsonProperty.ItemConverter = new EncryptionConverter(stringEncrypt);
                return jsonProperty;
            }

            return jsonProperty;
        }



    }
}