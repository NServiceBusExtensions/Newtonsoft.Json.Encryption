using System;
using System.Security.Cryptography;
using System.Threading;

namespace Newtonsoft.Json.Encryption
{
    public class ThreadLocalFactory :
        IDisposable
    {
        ThreadLocal<SessionState> local = new ThreadLocal<SessionState>();

        public IDisposable GetSession(SymmetricAlgorithm algorithm)
        {
            var sessionState = new SessionState(algorithm);
            local.Value = sessionState;
            return sessionState;
        }

        public ContractResolver GetContractResolver()
        {
            return new ContractResolver(
                encrypter: new Encrypter(
                    encryptProvider: () => local.Value.EncryptProvider(),
                    decryptProvider: () => local.Value.DecryptProvider(),
                    encryptCleanup: transform =>
                    {
                        local.Value.EncryptCleanup(transform);
                    },
                    decryptCleanup: transform =>
                    {
                        local.Value.DecryptCleanup(transform);
                    })
            );
        }

        public void Dispose()
        {
            local?.Dispose();
        }
    }
}