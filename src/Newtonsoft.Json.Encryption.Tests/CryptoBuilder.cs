using System.Security.Cryptography;

public static class CryptoBuilder
{
    public static SymmetricAlgorithm Build() =>
        new RijndaelManaged
        {
            IV = new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            Key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")
        };
}