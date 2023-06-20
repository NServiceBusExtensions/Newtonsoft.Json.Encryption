using Newtonsoft.Json.Encryption;
using NServiceBus.Pipeline;

class DecryptBehavior :
    IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
{
    EncryptionFactory factory;
    DecryptStateBuilder stateBuilder;

    public DecryptBehavior(EncryptionFactory factory, DecryptStateBuilder stateBuilder)
    {
        this.factory = factory;
        this.stateBuilder = stateBuilder;
    }

    public async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
    {
        if (!context.MessageHeaders.ReadKeyAndIv(out var keyId, out var iv))
        {
            await next(context);
            return;
        }
        using (factory.GetDecryptSession(stateBuilder(keyId!, iv)))
        {
            await next(context);
        }
    }
}