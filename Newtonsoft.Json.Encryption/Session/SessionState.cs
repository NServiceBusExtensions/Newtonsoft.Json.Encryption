using System;
using System.Security.Cryptography;

namespace Newtonsoft.Json.Encryption
{
    class SessionState : IDisposable
    {
        public SessionState(SymmetricAlgorithm algorithm)
        {
            EncryptProvider = () => encrypt ?? algorithm.CreateEncryptor();
            DecryptProvider = () => decrypt ?? algorithm.CreateDecryptor();
            EncryptCleanup = transform =>
            {
                if (!transform.CanReuseTransform)
                {
                    encrypt.Dispose();
                    encrypt = null;
                }
            };
            DecryptCleanup = transform =>
            {
                if (!transform.CanReuseTransform)
                {
                    decrypt.Dispose();
                    decrypt = null;
                }
            };
        }

        ICryptoTransform encrypt;
        public readonly Func<ICryptoTransform> EncryptProvider;
        public readonly Action<ICryptoTransform> EncryptCleanup;

        ICryptoTransform decrypt;
        public readonly Func<ICryptoTransform> DecryptProvider;
        public readonly Action<ICryptoTransform> DecryptCleanup;

        public void Dispose()
        {
            encrypt?.Dispose();
            decrypt?.Dispose();
        }
    }
}