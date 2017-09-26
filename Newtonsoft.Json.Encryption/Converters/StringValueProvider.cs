using Newtonsoft.Json.Encryption;
using Newtonsoft.Json.Serialization;

class StringValueProvider : IValueProvider
{
    Encrypter encrypter;
    IValueProvider wrappedProvider;

    public StringValueProvider(Encrypter encrypter, IValueProvider wrappedProvider)
    {
        this.encrypter = encrypter;
        this.wrappedProvider = wrappedProvider;
    }

    public object GetValue(object target)
    {
        var value = wrappedProvider.GetValue(target);
        return encrypter.Encrypt((string) value);
    }

    public void SetValue(object target, object value)
    {
        var decrypt = encrypter.Decrypt((string) value);
        wrappedProvider.SetValue(target, decrypt);
    }
}