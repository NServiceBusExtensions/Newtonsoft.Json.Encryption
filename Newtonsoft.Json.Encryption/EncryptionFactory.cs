using System;
using System.Security.Cryptography;
using System.Threading;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionFactory :
        IDisposable
    {
        AsyncLocal<CryptoState> encryptLocal = new AsyncLocal<CryptoState>();
        AsyncLocal<CryptoState> decryptLocal = new AsyncLocal<CryptoState>();

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
            encryptLocal?.Value?.Dispose();
            decryptLocal?.Value?.Dispose();
        }
    }
}