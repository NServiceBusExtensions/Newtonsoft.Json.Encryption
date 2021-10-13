using System.Security.Cryptography;

namespace Newtonsoft.Json.Encryption;

public delegate (SymmetricAlgorithm algorithm, string keyId) EncryptStateBuilder();
public delegate SymmetricAlgorithm DecryptStateBuilder(string keyId, byte[] initVector);