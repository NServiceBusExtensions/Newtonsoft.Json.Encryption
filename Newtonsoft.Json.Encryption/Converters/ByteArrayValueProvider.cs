using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Encryption;

class ByteArrayValueProvider : IValueProvider
{
    Encrypter encrypter;
    IValueProvider wrappedProvider;

    public ByteArrayValueProvider(Encrypter encrypter, IValueProvider wrappedProvider)
    {
        this.encrypter = encrypter;
        this.wrappedProvider = wrappedProvider;
    }

    public object GetValue(object target)
    {
        var value = wrappedProvider.GetValue(target);
        return encrypter.EncryptBytes((byte[])value);
    }

    public void SetValue(object target, object value)
    {
        var decrypt = encrypter.DecryptBytes((byte[])value);
        wrappedProvider.SetValue(target, decrypt);
    }
}