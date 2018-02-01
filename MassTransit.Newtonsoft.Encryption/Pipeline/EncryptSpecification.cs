using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using Newtonsoft.Json.Encryption;

namespace MassTransit.Newtonsoft.Encryption
{
    public class EncryptSpecification : IPipeSpecification<SendContext>
    {
        EncryptionFactory factory;
        EncryptStateBuilder stateBuilder;

        public EncryptSpecification(EncryptionFactory factory, EncryptStateBuilder stateBuilder)
        {
            this.factory = factory;
            this.stateBuilder = stateBuilder;
        }
        public void Apply(IPipeBuilder<SendContext> builder)
        {
            builder.AddFilter(new EncryptFilter(factory, stateBuilder));
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}