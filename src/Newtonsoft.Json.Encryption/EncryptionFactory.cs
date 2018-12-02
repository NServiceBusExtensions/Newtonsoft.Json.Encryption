using System;
using System.Security.Cryptography;
using System.Threading;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionFactory :
        IDisposable
    {
        AsyncLocal<CryptoState> encrypt = new AsyncLocal<CryptoState>();
        AsyncLocal<CryptoState> decrypt = new AsyncLocal<CryptoState>();

        public IDisposable GetEncryptSession(SymmetricAlgorithm algorithm)
        {
            return encrypt.Value = new CryptoState(algorithm.CreateEncryptor);
        }

        public IDisposable GetDecryptSession(SymmetricAlgorithm algorithm)
        {
            return decrypt.Value = new CryptoState(algorithm.CreateDecryptor);
        }

        public ContractResolver GetContractResolver()
        {
            return new ContractResolver(
                encrypter: new Encrypter(
                    encryptProvider: () => encrypt.Value.Provider(),
                    decryptProvider: () => decrypt.Value.Provider(),
                    encryptCleanup: transform =>
                    {
                        encrypt.Value.Cleanup(transform);
                    },
                    decryptCleanup: transform =>
                    {
                        decrypt.Value.Cleanup(transform);
                    })
            );
        }

        public void Dispose()
        {
            encrypt?.Value?.Dispose();
            decrypt?.Value?.Dispose();
        }
    }
}