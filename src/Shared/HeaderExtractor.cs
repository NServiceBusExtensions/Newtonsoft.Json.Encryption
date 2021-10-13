using Newtonsoft.Json.Encryption;

static class HeaderExtractor
{
    public static bool ReadKeyAndIv(this IReadOnlyDictionary<string, string> headers, out string keyId, out byte[] iv)
    {
        headers.TryGetValue(KeyId, out keyId);
        headers.TryGetValue(Iv, out var ivString);
        var ivIsEmpty = string.IsNullOrWhiteSpace(ivString);
        var keyIdIsEmpty = string.IsNullOrWhiteSpace(keyId);
        if (!keyIdIsEmpty && !ivIsEmpty)
        {
            iv = Convert.FromBase64String(ivString);
            return true;
        }
        if (keyIdIsEmpty && ivIsEmpty)
        {
            iv = Array.Empty<byte>();
            return false;
        }
        throw new KeyIdAndIvHeaderMismatchException();
    }

    public static void WriteKeyAndIv(this IDictionary<string, string> headers, string keyId, byte[] iv)
    {
        headers[KeyId] = keyId;
        headers[Iv] = Convert.ToBase64String(iv);
    }

    internal const string KeyId = "NewtonsoftEncryptionKeyId";
    internal const string Iv = "NewtonsoftEncryptionIv";
}