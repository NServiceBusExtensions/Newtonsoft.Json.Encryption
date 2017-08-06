using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public static class ValueProviderCreater {

        public static bool TryCreate(
            MemberInfo member,
            StringEncrypt stringEncrypt,
            out IValueProvider provider)
        {
            if (member.GetCustomAttribute<EncryptAttribute>() == null)
            {
                provider = null;
                return false;
            }
            if (member.GetUnderlyingType() == typeof(string))
            {
                provider = new StringValueProvider(
                    targetMember: member,
                    stringEncrypt: stringEncrypt);
                return true;
            }
            throw new Exception("Expected a string");
        }
    }
}