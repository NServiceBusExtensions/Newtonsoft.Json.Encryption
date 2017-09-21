using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class StringValueProvider : IValueProvider
    {
        MemberInfo member;
        Encrypter encrypter;

        public StringValueProvider(MemberInfo member, Encrypter encrypter)
        {
            this.member = member;
            this.encrypter = encrypter;
        }

        public object GetValue(object target)
        {
            var value = member.GetValue(target);
            return encrypter.Encrypt((string)value);
        }

        public void SetValue(object target, object value)
        {
            var decrypt = encrypter.Decrypt((string)value);
            member.SetValue(target, decrypt);
        }
    }
}