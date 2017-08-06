using System;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionContractResolver : DefaultContractResolver
    {
        Func<ICryptoTransform> encryptProvider;
        Func<ICryptoTransform> decryptProvider;
        Action<ICryptoTransform> cryptoCleanup;

        public EncryptionContractResolver(Func<ICryptoTransform> encryptProvider, Func<ICryptoTransform> decryptProvider, Action<ICryptoTransform> cryptoCleanup)
        {
            this.encryptProvider = encryptProvider;
            this.decryptProvider = decryptProvider;
            this.cryptoCleanup = cryptoCleanup;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty == null)
            {
                return null;
            }
            if (EncryptionStringValueProvider.TryCreate(
                member: member,
                encryptProvider: encryptProvider,
                decryptProvider: decryptProvider,
                cryptoCleanup: cryptoCleanup, provider: out var provider))
            {
                jsonProperty.ValueProvider = provider;
            }
            return jsonProperty;
        }
    }
}