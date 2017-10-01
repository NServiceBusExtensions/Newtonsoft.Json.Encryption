using System.Threading.Tasks;
using GreenPipes;
using Newtonsoft.Json.Encryption;

namespace MassTransit.Newtonsoft.Encryption
{
    public class DecryptFilter :
        IFilter<ConsumeContext>
    {
        EncryptionFactory factory;
        DecryptStateBuilder stateBuilder;

        public DecryptFilter(EncryptionFactory factory, DecryptStateBuilder stateBuilder)
        {
            this.factory = factory;
            this.stateBuilder = stateBuilder;
        }
        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            if (!context.ReceiveContext.TransportHeaders.ReadKeyAndIv(out var keyId, out var iv))
            {
                await next.Send(context).ConfigureAwait(false);
                return;
            }
            using (factory.GetDecryptSession(stateBuilder(keyId, iv)))
            {
                await next.Send(context).ConfigureAwait(false);
            }
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}