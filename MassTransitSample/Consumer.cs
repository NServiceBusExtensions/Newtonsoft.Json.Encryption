using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitSample;

public class Consumer : IConsumer<MessageWithSecretData>
{
    public Task Consume(ConsumeContext<MessageWithSecretData> context)
    {
        var message = context.Message;
        Console.WriteLine($"Secret: '{message.Secret}'");
        Console.WriteLine($"SubSecret: {message.SubProperty.Secret}");
        foreach (var creditCard in message.CreditCards)
        {
            Console.WriteLine($"CreditCard: {creditCard.Number} is valid to {creditCard.ValidTo}");
        }
        return Task.CompletedTask;
    }
}