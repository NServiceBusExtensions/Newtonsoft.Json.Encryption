using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class ContractResolver : DefaultContractResolver
    {
        Encrypter encrypter;

        public ContractResolver(Encrypter encrypter)
        {
            this.encrypter = encrypter;
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
                encrypter: encrypter,
                jsonProperty: jsonProperty);
            return jsonProperty;
        }
    }
}