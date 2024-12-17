namespace StocksNotification.Stocks.Events.Consumers;

public class PriceAlertTriggeredConsumer(EmailService emailService) : IConsumer<PriceAlertTriggeredEvent>
{
    public async Task Consume(ConsumeContext<PriceAlertTriggeredEvent> context)
    {
        var message = context.Message;
        await emailService.SendEmailAsync(message);
    }
}