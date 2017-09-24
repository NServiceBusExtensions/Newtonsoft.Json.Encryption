using System.Reflection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Encryption;

class ByteArrayValueProvider : IValueProvider
{
    MemberInfo targetMember;
    Encrypter encrypter;

    public ByteArrayValueProvider(
        MemberInfo targetMember,
        Encrypter encrypter)
    {
        this.targetMember = targetMember;
        this.encrypter = encrypter;
    }

    public object GetValue(object target)
    {
        var value = targetMember.GetValue(target);
        return encrypter.EncryptBytes((byte[])value);
    }

    public void SetValue(object target, object value)
    {
        var decrypt = encrypter.DecryptBytes((byte[])value);
        targetMember.SetValue(target, decrypt);
    }
}