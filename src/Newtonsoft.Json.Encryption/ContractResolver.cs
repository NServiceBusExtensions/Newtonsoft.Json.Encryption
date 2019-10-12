using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class ContractResolver :
        DefaultContractResolver
    {
        Encrypter encrypter;

        public ContractResolver(Encrypter encrypter)
        {
            this.encrypter = encrypter;
        }

        protected override JsonProperty? CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property == null)
            {
                return null;
            }
            JsonPropertyHelper.Manipulate(member, encrypter, property);
            return property;
        }
    }
}