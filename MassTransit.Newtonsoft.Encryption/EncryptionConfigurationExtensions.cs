using Newtonsoft.Json.Encryption;

namespace MassTransit.Newtonsoft.Encryption
{
    public static class EncryptionConfigurationExtensions
    {
        public static void EnableJsonEncryption(this IBusFactoryConfigurator configurator, EncryptionFactory encryptionFactory, EncryptStateBuilder encryptStateBuilder, DecryptStateBuilder decryptStateBuilder)
        {
            Guard.AgainstNull(nameof(configurator), configurator);
            Guard.AgainstNull(nameof(encryptionFactory), encryptionFactory);
            Guard.AgainstNull(nameof(encryptStateBuilder), encryptStateBuilder);
            Guard.AgainstNull(nameof(decryptStateBuilder), decryptStateBuilder);
            configurator.ConfigureSend(config =>
            {
                config.AddPipeSpecification(new EncryptSpecification(encryptionFactory, encryptStateBuilder));
            });
            configurator.ConfigurePublish(config =>
            {
                config.AddPipeSpecification(new EncryptSpecification(encryptionFactory, encryptStateBuilder));
            });
            configurator.AddPipeSpecification(new DecryptSpecification(encryptionFactory, decryptStateBuilder));
        }
    }
}