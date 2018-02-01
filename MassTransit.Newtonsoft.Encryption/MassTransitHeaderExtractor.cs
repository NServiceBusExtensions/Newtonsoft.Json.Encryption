using System;
using MassTransit;
using Newtonsoft.Json.Encryption;

static class MassTransitHeaderExtractor
{
    static string TryGetHeader(this Headers headers, string key)
    {
        headers.TryGetHeader(key, out var value);
        return (string) value;
    }

    public static bool ReadKeyAndIv(this Headers headers, out string keyId, out byte[] iv)
    {
        keyId = headers.TryGetHeader(HeaderExtractor.KeyId);
        var ivString = headers.TryGetHeader(HeaderExtractor.Iv);
        var ivIsEmpty = string.IsNullOrWhiteSpace(ivString);
        var keyIdIsEmpty = string.IsNullOrWhiteSpace(keyId);
        if (!keyIdIsEmpty && !ivIsEmpty)
        {
            iv = Convert.FromBase64String(ivString);
            return true;
        }
        if (keyIdIsEmpty && ivIsEmpty)
        {
            iv = null;
            return false;
        }
        throw new KeyIdAndIvHeaderMismatchException();
    }
    public static void WriteKeyAndIv(this SendHeaders headers, string keyId, byte[] iv)
    {
        headers.Set(HeaderExtractor.KeyId, keyId);
        headers.Set(HeaderExtractor.Iv, Convert.ToBase64String(iv));
    }
}