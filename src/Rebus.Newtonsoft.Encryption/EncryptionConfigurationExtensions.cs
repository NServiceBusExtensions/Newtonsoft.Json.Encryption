using Newtonsoft.Json.Encryption;
using Rebus.Config;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Rebus.Pipeline.Send;

namespace Rebus.Newtonsoft.Encryption
{
    /// <summary>
    /// Configuration extensions for enabling encrypted message bodies
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Configures Rebus to encrypt messages.
        /// </summary>
        public static void EnableJsonEncryption(this RebusConfigurer configurer, EncryptionFactory encryptionFactory, EncryptStateBuilder encryptStateBuilder, DecryptStateBuilder decryptStateBuilder)
        {
            configurer.Options(options =>
            {
                options.Decorate<IPipeline>(
                    resolutionContext =>
                    {
                        var pipeline = resolutionContext.Get<IPipeline>();
                        var injector = new PipelineStepInjector(pipeline);

                        var decryptStep = new DecryptStep(encryptionFactory, decryptStateBuilder);
                        injector.OnReceive(decryptStep, PipelineRelativePosition.Before, typeof(DeserializeIncomingMessageStep));

                        var encryptStep = new EncryptStep(encryptionFactory, encryptStateBuilder);
                        injector.OnSend(encryptStep, PipelineRelativePosition.Before, typeof(SerializeOutgoingMessageStep));

                        return injector;
                    });
            });
        }
    }
}