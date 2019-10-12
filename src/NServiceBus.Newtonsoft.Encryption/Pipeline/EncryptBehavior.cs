using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Encryption;
using NServiceBus.Pipeline;

class EncryptBehavior :
    IBehavior<IOutgoingLogicalMessageContext, IOutgoingLogicalMessageContext>
{
    EncryptionFactory factory;
    EncryptStateBuilder stateBuilder;

    public EncryptBehavior(EncryptionFactory factory, EncryptStateBuilder stateBuilder)
    {
        this.factory = factory;
        this.stateBuilder = stateBuilder;
    }

    public async Task Invoke(IOutgoingLogicalMessageContext context, Func<IOutgoingLogicalMessageContext, Task> next)
    {
        var state = stateBuilder();
        context.Headers.WriteKeyAndIv(state.keyId, state.algorithm.IV);
        using (factory.GetEncryptSession(state.algorithm))
        {
            await next(context)
                ;
        }
    }
}