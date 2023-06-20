﻿using NServiceBus.Logging;

public class Handler :
    IHandleMessages<MessageWithSecretData>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        log.Info($"Secret: '{message.Secret}'");
        log.Info($"SubSecret: {message.SubProperty?.Secret}");
        if (message.CreditCards != null)
        {
            foreach (var creditCard in message.CreditCards)
            {
                log.Info($"CreditCard: {creditCard.Number} is valid to {creditCard.ValidTo}");
            }
        }

        return Task.CompletedTask;
    }
}