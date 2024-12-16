namespace StocksMonitorService.Stocks.Events.Handlers;

[MessageUrn("price-alert-triggered-event")]
public record PriceAlertTriggeredEvent: INotification
{
    public string? SubscriberName { get; set; } = null!;
    public string? SubscriberEmail { get; set; } = null!;
    public string? StockName { get; set; } = null!;
    public decimal? SellingPrice { get; set; }
    public decimal? BuyingPrice { get; set; }
    public decimal? StockHighPrice { get; set; }
    public decimal? StockLowPrice { get; set; }
    public DateTime? StockDateTime { get; set; }
}

public class PriceAlertTriggeredHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<PriceAlertTriggeredEvent>
{
    public async Task Handle(PriceAlertTriggeredEvent notification, CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish(notification, cancellationToken);
    }
}