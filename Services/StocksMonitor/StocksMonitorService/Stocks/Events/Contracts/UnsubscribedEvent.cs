namespace StocksMonitorService.Stocks.Events.Contracts;

[MessageUrn("unsubscribed-event")]
public record UnsubscribedEvent
{
    public string SubscriberName { get; set; } = null!;
    public string SubscriberEmail { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    public decimal BuyingPrice { get; set; }
    public DateTime UnsubscribedAt { get; private set; } = DateTime.UtcNow;
    public bool IsNotificationEnabled { get; private set; } = false;
}