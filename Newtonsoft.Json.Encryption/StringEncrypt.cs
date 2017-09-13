using System;
using System.IO;
using System.Security.Cryptography;

namespace Newtonsoft.Json.Encryption
{
    public class StringEncrypt
    {
        Func<ICryptoTransform> encryptProvider;
        Func<ICryptoTransform> decryptProvider;
        Action<ICryptoTransform> encryptCleanup;
        Action<ICryptoTransform> decryptCleanup;

        public StringEncrypt(
            Func<ICryptoTransform> encryptProvider,
            Func<ICryptoTransform> decryptProvider,
            Action<ICryptoTransform> encryptCleanup,
            Action<ICryptoTransform> decryptCleanup)
        {
            this.encryptProvider = encryptProvider;
            this.decryptProvider = decryptProvider;
            this.encryptCleanup = encryptCleanup;
            this.decryptCleanup = decryptCleanup;
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
                encryptCleanup(encryptor);
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
                decryptCleanup(decryptor);
            }
        }

        public byte[] EncryptBytes(byte[] target)
        {
            var encryptor = encryptProvider();
            try
            {
                return PerformCryptography(encryptor, target);
            }
            finally
            {
                encryptCleanup(encryptor);
            }
        }

        public byte[] DecryptBytes(byte[] value)
        {
            var decryptor = decryptProvider();

            try
            {
                return PerformCryptography(decryptor, value);
            }
            finally
            {
                decryptCleanup(decryptor);
            }
        }

        byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }
                return memoryStream.ToArray();
            }
        }

    }
}