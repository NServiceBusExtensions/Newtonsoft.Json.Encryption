using NServiceBus.Features;
using NServiceBus.Json.Encryption;

class EncryptionFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        var pipeline = context.Pipeline;
        var encryptionFactory = settings.GetEncryptionFactory();
        pipeline.Register(new EncryptRegistration(encryptionFactory, settings.GetEncryptStateBuilder()));
        pipeline.Register(new DecryptRegistration(encryptionFactory, settings.GetDecryptStateBuilder()));
    }
}