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
            if (ValueProviderCreater.TryCreate(
                member: member,
                stringEncrypt: stringEncrypt,
                provider: out var provider))
            {
                jsonProperty.ValueProvider = provider;
            }
            jsonProperty.ItemConverter = new DictionaryItemConverter(stringEncrypt);
            return jsonProperty;
        }



    }
}