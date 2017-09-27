using Newtonsoft.Json.Encryption;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Settings;

namespace NServiceBus.Newtonsoft.Encryption
{
    /// <summary>
    /// Provides configuration extensions to configure message property encryption.
    /// </summary>
    public static class EncryptionConfigurationExtensions
    {
        /// <summary>
        /// Enable message property encryption.
        /// </summary>
        public static void EnableJsonEncryption(this EndpointConfiguration configuration, EncryptionFactory encryptionFactory, EncryptStateBuilder encryptStateBuilder, DecryptStateBuilder decryptStateBuilder)
        {
            Guard.AgainstNull(nameof(configuration), configuration);
            Guard.AgainstNull(nameof(encryptionFactory), encryptionFactory);
            Guard.AgainstNull(nameof(encryptStateBuilder), encryptStateBuilder);
            Guard.AgainstNull(nameof(decryptStateBuilder), decryptStateBuilder);

            var recoverability = configuration.Recoverability();
            recoverability.AddUnrecoverableException<KeyIdAndIvHeaderMismatchException>();
            var settings = configuration.GetSettings();
            settings.Set<EncryptStateBuilder>(encryptStateBuilder);
            settings.Set<DecryptStateBuilder>(decryptStateBuilder);
            settings.Set<EncryptionFactory>(encryptionFactory);
            configuration.EnableFeature<EncryptionFeature>();
        }

        internal static EncryptionFactory GetEncryptionFactory(this ReadOnlySettings settings)
        {
            return settings.Get<EncryptionFactory>();
        }

        internal static EncryptStateBuilder GetEncryptStateBuilder(this ReadOnlySettings settings)
        {
            return settings.Get<EncryptStateBuilder>();
        }

        internal static DecryptStateBuilder GetDecryptStateBuilder(this ReadOnlySettings settings)
        {
            return settings.Get<DecryptStateBuilder>();
        }
    }
}