namespace Stocks.API.Stock.Events;

[MessageUrn("unsubscribed-event")]
public record UnsubscribedEvent : INotification
{
    public string SubscriberName { get; set; } = null!;
    public string SubscriberEmail { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    public decimal BuyingPrice { get; set; }
    public DateTime UnsubscribedAt { get; private set; } = DateTime.UtcNow;
    public bool IsNotificationEnabled { get; private set; } = false;
}

public class UnsubscribedHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<UnsubscribedEvent>
{
    public async Task Handle(UnsubscribedEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(notification, cancellationToken);
    }
}