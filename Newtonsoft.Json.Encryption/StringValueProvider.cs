using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class StringValueProvider : IValueProvider
    {
        MemberInfo targetMember;
        Encrypter encrypter;

        public StringValueProvider(
            MemberInfo targetMember,
            Encrypter encrypter)
        {
            this.targetMember = targetMember;
            this.encrypter = encrypter;
        }

        public object GetValue(object target)
        {
            var value = targetMember.GetValue(target);
            return encrypter.Encrypt((string)value);
        }

        public void SetValue(object target, object value)
        {
             targetMember.SetValue(target, encrypter.Decrypt((string)value));
        }

    }
}