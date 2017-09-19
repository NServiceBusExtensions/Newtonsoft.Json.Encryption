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

        class State : IDisposable
        {
            public State(SymmetricAlgorithm algorithm)
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

        private static State Current => local.Value;

        public static Func<ICryptoTransform> GetEncryptProvider()
        {
            return () => Current.EncryptProvider();
        }

        public static Action<ICryptoTransform> GetEncryptCleanup()
        {
            return transform =>
            {
                Current.EncryptCleanup(transform);
            };
        }

        public static Func<ICryptoTransform> GetDecryptProvider()
        {
            return () => Current.DecryptProvider();
        }

        public static Action<ICryptoTransform> GetDecryptCleanup()
        {
            return transform =>
            {
                Current.DecryptCleanup(transform);
            };
        }

        public void Dispose()
        {
            algorithm.Dispose();
            local.Value?.Dispose();
        }
    }
}