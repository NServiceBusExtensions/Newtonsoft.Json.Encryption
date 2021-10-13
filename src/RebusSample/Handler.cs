using Rebus.Handlers;

public class Handler :
    IHandleMessages<MessageWithSecretData>
{

    public Task Handle(MessageWithSecretData message)
    {
        Console.WriteLine($"Secret: '{message.Secret}'");
        Console.WriteLine($"SubSecret: {message.SubProperty?.Secret}");
        if (message.CreditCards != null)
        {
            foreach (var creditCard in message.CreditCards)
            {
                Console.WriteLine($"CreditCard: {creditCard.Number} is valid to {creditCard.ValidTo}");
            }
        }

        return Task.CompletedTask;
    }
}