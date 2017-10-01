using System.Threading.Tasks;
using GreenPipes;
using Newtonsoft.Json.Encryption;

namespace MassTransit.Newtonsoft.Encryption
{
    public class EncryptFilter :
        IFilter<SendContext>
    {
        EncryptionFactory factory;
        EncryptStateBuilder stateBuilder;

        public EncryptFilter(EncryptionFactory factory, EncryptStateBuilder stateBuilder)
        {
            this.factory = factory;
            this.stateBuilder = stateBuilder;
        }
        public async Task Send(SendContext context, IPipe<SendContext> next)
        {
            var state = stateBuilder();
            context.Headers.WriteKeyAndIv(state.keyId, state.algorithm.IV);
            using (factory.GetEncryptSession(state.algorithm))
            {
                await next.Send(context)
                    .ConfigureAwait(false);
            }
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}