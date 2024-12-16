namespace Stocks.API.Stock.Features.CreateSubscription;

public record SubscriptionDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    public decimal BuyingPrice { get; set; }
}