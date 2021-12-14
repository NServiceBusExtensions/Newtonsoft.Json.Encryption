using System.Security.Cryptography;

class CryptoState :
    IDisposable
{
    public CryptoState(Func<ICryptoTransform> encryptProvider)
    {
        Provider = () =>
        {
            if (transform == null)
            {
                return transform = encryptProvider();
            }
            return transform;
        };
        Cleanup = transform =>
        {
            if (!transform.CanReuseTransform)
            {
                this.transform?.Dispose();
                this.transform = null;
            }
        };
    }

    ICryptoTransform? transform;
    public readonly Func<ICryptoTransform> Provider;
    public readonly Action<ICryptoTransform> Cleanup;

    public void Dispose()
    {
        transform?.Dispose();
    }
}