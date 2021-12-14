using System.Security.Cryptography;

namespace Newtonsoft.Json.Encryption
{
    public class EncryptionFactory :
        IDisposable
    {
        AsyncLocal<CryptoState> encrypt = new();
        AsyncLocal<CryptoState> decrypt = new();

        public IDisposable GetEncryptSession(SymmetricAlgorithm algorithm)
        {
            return encrypt.Value = new(algorithm.CreateEncryptor);
        }

        public IDisposable GetDecryptSession(SymmetricAlgorithm algorithm)
        {
            return decrypt.Value = new(algorithm.CreateDecryptor);
        }

        public ContractResolver GetContractResolver()
        {
            return new(
                encrypter: new(
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