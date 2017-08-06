using System.Security.Cryptography;
using System.Text;

public static class CryptoBuilder
{

    public static SymmetricAlgorithm Build()
    {
        return new RijndaelManaged
        {
            IV = new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            Key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")
        };

    }
}