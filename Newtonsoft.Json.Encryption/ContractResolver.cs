using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class ContractResolver : DefaultContractResolver
    {
        StringEncrypt stringEncrypt;

        public ContractResolver(StringEncrypt stringEncrypt)
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
            JsonPropertyHelper.Manipulate(
                member: member,
                stringEncrypt: stringEncrypt,
                jsonProperty: jsonProperty);
            return jsonProperty;
        }



    }
}