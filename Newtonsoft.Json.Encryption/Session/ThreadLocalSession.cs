using System;
using System.Security.Cryptography;
using System.Threading;

namespace Newtonsoft.Json.Encryption
{
    public class ThreadLocalSession :
        IDisposable
    {
        SymmetricAlgorithm algorithm;
        static ThreadLocal<State> local = new ThreadLocal<State>();

        public ThreadLocalSession(SymmetricAlgorithm algorithm)
        {
            this.algorithm = algorithm;
            local.Value = new State(algorithm);
        }

        class State:IDisposable
        {
            public State(SymmetricAlgorithm algorithm)
            {
                EncryptProvider = () => Encrypt ?? algorithm.CreateEncryptor();
                DecryptProvider = () => Decrypt ?? algorithm.CreateDecryptor();
                EncryptCleanup = transform =>
                {
                    if (!transform.CanReuseTransform)
                    {
                        Encrypt.Dispose();
                        Encrypt = null;
                    }
                };
                DecryptCleanup = transform =>
                {
                    if (!transform.CanReuseTransform)
                    {
                        Decrypt.Dispose();
                        Decrypt = null;
                    }
                };
            }

            public readonly Func<ICryptoTransform> EncryptProvider;
            ICryptoTransform Encrypt;
            public readonly Action<ICryptoTransform> EncryptCleanup;

            public readonly Func<ICryptoTransform> DecryptProvider;
            ICryptoTransform Decrypt;
            public readonly Action<ICryptoTransform> DecryptCleanup;

            public void Dispose()
            {
                Encrypt?.Dispose();
                Decrypt?.Dispose();
            }
        }

        public void Dispose()
        {
            local.Value?.Dispose();
        }
    }
}