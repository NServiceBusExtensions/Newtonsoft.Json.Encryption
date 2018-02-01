using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using Newtonsoft.Json.Encryption;

namespace MassTransit.Newtonsoft.Encryption
{
    public class DecryptSpecification : IPipeSpecification<ConsumeContext>
    {
        EncryptionFactory factory;
        DecryptStateBuilder stateBuilder;

        public DecryptSpecification(EncryptionFactory factory, DecryptStateBuilder stateBuilder)
        {
            this.factory = factory;
            this.stateBuilder = stateBuilder;
        }
        public void Apply(IPipeBuilder<ConsumeContext> builder)
        {
            builder.AddFilter(new DecryptFilter(factory, stateBuilder));
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}