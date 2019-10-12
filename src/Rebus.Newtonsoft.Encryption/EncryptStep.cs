using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Encryption;
using Rebus.Messages;
using Rebus.Pipeline;

[StepDocumentation("Encrypts nested properties of the outgoing message")]
class EncryptStep :
    IOutgoingStep
{
    EncryptionFactory factory;
    EncryptStateBuilder stateBuilder;

    public EncryptStep(EncryptionFactory factory, EncryptStateBuilder stateBuilder)
    {
        this.factory = factory;
        this.stateBuilder = stateBuilder;
    }

    public async Task Process(OutgoingStepContext context, Func<Task> next)
    {
        var message = context.Load<Message>();
        var state = stateBuilder();
        message.Headers.WriteKeyAndIv(state.keyId, state.algorithm.IV);
        using (factory.GetEncryptSession(state.algorithm))
        {
            await next();
        }
    }
}