using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionStringValueProvider : IValueProvider
    {
        MemberInfo targetMember;
        Func<ICryptoTransform> encryptProvider;
        Func<ICryptoTransform> decryptProvider;
        Action<ICryptoTransform> cryptoCleanup;

        public EncryptionStringValueProvider(
            MemberInfo targetMember,
            Func<ICryptoTransform> encryptProvider,
            Func<ICryptoTransform> decryptProvider,
            Action<ICryptoTransform> cryptoCleanup)
        {
            this.targetMember = targetMember;
            this.encryptProvider = encryptProvider;
            this.decryptProvider = decryptProvider;
            this.cryptoCleanup = cryptoCleanup;
        }

        public object GetValue(object target)
        {
            var value = targetMember.GetValue(target);
            var encryptor = encryptProvider();
            try
            {
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(value);
                    writer.Flush();
                    cryptoStream.Flush();
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            finally
            {
                cryptoCleanup(encryptor);
            }
        }

        public void SetValue(object target, object value)
        {
            var encrypted = Convert.FromBase64String((string) value);
            var decryptor = decryptProvider();
            try
            {
                using (var memoryStream = new MemoryStream(encrypted))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    targetMember.SetValue(target, reader.ReadToEnd());
                }
            }
            finally
            {
                cryptoCleanup(decryptor);
            }
        }

        public static bool TryCreate(
            MemberInfo member,
            Func<ICryptoTransform> encryptProvider,
            Func<ICryptoTransform> decryptProvider,
            Action<ICryptoTransform> cryptoCleanup,
            out EncryptionStringValueProvider provider)
        {
            if (member.GetCustomAttribute<EncryptAttribute>() == null)
            {
                provider = null;
                return false;
            }
            if (member.GetUnderlyingType() != typeof(string))
            {
                throw new Exception("Expected a string");
            }
            provider = new EncryptionStringValueProvider(
                targetMember: member,
                encryptProvider: encryptProvider,
                decryptProvider: decryptProvider,
                cryptoCleanup: cryptoCleanup);
            return true;
        }

    }
}