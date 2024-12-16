namespace StocksMonitorService.Stocks.Events.Contracts;

[MessageUrn("subscribed-event")]
public record SubscribedEvent
{
    public string SubscriberName { get; set; } = null!;
    public string SubscriberEmail { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    public decimal BuyingPrice { get; set; }
    public DateTime SubscribedAt { get; set; }
    public bool IsNotificationEnabled { get; private set; } = true;
}