using Newtonsoft.Json.Encryption;
using NServiceBus.Pipeline;

class DecryptRegistration : RegisterStep
{
    public DecryptRegistration(EncryptionFactory encryptionFactory, DecryptStateBuilder stateBuilder)
        : base(
            stepId: "NServiceBusJsonEncryptionDecrypt",
            behavior: typeof(DecryptBehavior),
            description: "Invokes the decryption logic",
            factoryMethod: _ => new DecryptBehavior(encryptionFactory, stateBuilder))
    {
        InsertBefore("MutateIncomingTransportMessage");
    }
}