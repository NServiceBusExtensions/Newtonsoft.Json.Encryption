using System;
using System.IO;
using System.Security.Cryptography;

namespace Newtonsoft.Json.Encryption
{
    public class StringEncrypt
    {
        Func<ICryptoTransform> encryptProvider;
        Func<ICryptoTransform> decryptProvider;
        Action<ICryptoTransform> cryptoCleanup;

        public StringEncrypt(
            Func<ICryptoTransform> encryptProvider,
            Func<ICryptoTransform> decryptProvider,
            Action<ICryptoTransform> cryptoCleanup)
        {
            this.encryptProvider = encryptProvider;
            this.decryptProvider = decryptProvider;
            this.cryptoCleanup = cryptoCleanup;
        }

        public string Encrypt(string target)
        {
            var encryptor = encryptProvider();
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(target);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            finally
            {
                cryptoCleanup(encryptor);
            }
        }

        public string Decrypt(string value)
        {
            var decryptor = decryptProvider();
            var encrypted = Convert.FromBase64String(value);
            try
            {
                using (var memoryStream = new MemoryStream(encrypted))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                cryptoCleanup(decryptor);
            }
        }

        public byte[] EncryptBytes(byte[] target)
        {
            var encryptor = encryptProvider();
            return PerformCryptography(encryptor, target);
        }

        public byte[] DecryptBytes(byte[] value)
        {
            var decryptor = decryptProvider();
            return PerformCryptography(decryptor, value);
        }

        byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
            finally
            {
                cryptoCleanup(cryptoTransform);
            }
        }

    }
}