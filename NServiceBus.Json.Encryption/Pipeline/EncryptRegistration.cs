using Newtonsoft.Json.Encryption;
using NServiceBus.Pipeline;

class EncryptRegistration : RegisterStep
{
    public EncryptRegistration(EncryptionFactory encryptionFactory, EncryptStateBuilder stateBuilder)
        : base(
            stepId: "NServiceBusJsonEncryptionEncrypt",
            behavior: typeof(EncryptBehavior),
            description: "Invokes the encrypt logic",
            factoryMethod: _ => new EncryptBehavior(encryptionFactory, stateBuilder))
    {
        InsertAfter("MutateOutgoingMessages");
    }
}