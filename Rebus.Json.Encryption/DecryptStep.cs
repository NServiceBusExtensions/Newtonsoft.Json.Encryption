using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Encryption;
using Rebus.Messages;
using Rebus.Pipeline;

[StepDocumentation("Decrypts nested properties of the incoming message")]
class DecryptStep : IIncomingStep
{
    EncryptionFactory factory;
    DecryptStateBuilder stateBuilder;

    public DecryptStep(EncryptionFactory factory, DecryptStateBuilder stateBuilder)
    {
        this.factory = factory;
        this.stateBuilder = stateBuilder;
    }

    public async Task Process(IncomingStepContext context, Func<Task> next)
    {
        var transportMessage = context.Load<TransportMessage>();

        if (!transportMessage.Headers.ReadKeyAndIv(out var keyId, out var iv))
        {
            await next().ConfigureAwait(false);
            return;
        }
        using (factory.GetDecryptSession(stateBuilder(keyId, iv)))
        {
            await next().ConfigureAwait(false);
        }
    }
}