namespace StocksNotification.Stocks.Events.Contracts;

[MessageUrn("price-alert-triggered-event")]
public record PriceAlertTriggeredEvent
{
    public string SubscriberName { get; set; } = null!;
    public string SubscriberEmail { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    public decimal BuyingPrice { get; set; }
    public decimal StockHighPrice { get; set; }
    public decimal StockLowPrice { get; set; }
    public DateTime StockDateTime { get; set; }
}