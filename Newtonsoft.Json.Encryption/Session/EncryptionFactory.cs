using System;
using System.Security.Cryptography;
using System.Threading;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionFactory :
        IDisposable
    {
        ThreadLocal<CryptoState> encryptLocal = new ThreadLocal<CryptoState>();
        ThreadLocal<CryptoState> decryptLocal = new ThreadLocal<CryptoState>();

        public IDisposable GetEncryptSession(SymmetricAlgorithm algorithm)
        {
            var sessionState = new CryptoState(algorithm.CreateEncryptor);
            encryptLocal.Value = sessionState;
            return sessionState;
        }

        public IDisposable GetDecryptSession(SymmetricAlgorithm algorithm)
        {
            var sessionState = new CryptoState(algorithm.CreateDecryptor);
            decryptLocal.Value = sessionState;
            return sessionState;
        }

        public ContractResolver GetContractResolver()
        {
            return new ContractResolver(
                encrypter: new Encrypter(
                    encryptProvider: () => encryptLocal.Value.Provider(),
                    decryptProvider: () => decryptLocal.Value.Provider(),
                    encryptCleanup: transform =>
                    {
                        encryptLocal.Value.Cleanup(transform);
                    },
                    decryptCleanup: transform =>
                    {
                        decryptLocal.Value.Cleanup(transform);
                    })
            );
        }

        public void Dispose()
        {
            encryptLocal?.Dispose();
            decryptLocal?.Dispose();
        }
    }
}