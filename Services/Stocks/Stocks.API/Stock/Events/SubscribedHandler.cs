namespace Stocks.API.Stock.Events;

[MessageUrn("subscribed-event")]
public record SubscribedEvent : INotification
{
    public string SubscriberName { get; set; } = null!;
    public string SubscriberEmail { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    public decimal BuyingPrice { get; set; }
    public DateTime SubscribedAt { get; private set; } = DateTime.UtcNow;
    public bool IsNotificationEnabled { get; private set; } = true;
}

public class SubscribedHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<SubscribedEvent>
{
    public async Task Handle(SubscribedEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(notification, cancellationToken);
    }
}