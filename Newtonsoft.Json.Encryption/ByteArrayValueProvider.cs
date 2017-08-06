using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class ByteArrayValueProvider : IValueProvider
    {
        MemberInfo targetMember;
        StringEncrypt stringEncrypt;

        public ByteArrayValueProvider(
            MemberInfo targetMember,
            StringEncrypt stringEncrypt)
        {
            this.targetMember = targetMember;
            this.stringEncrypt = stringEncrypt;
        }

        public object GetValue(object target)
        {
            var value = targetMember.GetValue(target);
            return stringEncrypt.EncryptBytes((byte[])value);
        }

        public void SetValue(object target, object value)
        {
            var decrypt = stringEncrypt.DecryptBytes((byte[])value);
            targetMember.SetValue(target, decrypt);
        }

    }
}