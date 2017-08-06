using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class StringValueProvider : IValueProvider
    {
        MemberInfo targetMember;
        StringEncrypt stringEncrypt;

        public StringValueProvider(
            MemberInfo targetMember,
            StringEncrypt stringEncrypt)
        {
            this.targetMember = targetMember;
            this.stringEncrypt = stringEncrypt;
        }

        public object GetValue(object target)
        {
            var value = targetMember.GetValue(target);
            return stringEncrypt.Encrypt(value);
        }

        public void SetValue(object target, object value)
        {
             targetMember.SetValue(target, stringEncrypt.Decrypt((string)value));
        }

    }
}