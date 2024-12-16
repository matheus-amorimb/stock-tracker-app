namespace Stocks.API.Stock.Models;

public class StocksModel : Entity<Guid>
{
    public Name SubscriberName { get; private set; } = null!;
    public Email SubscriberEmail { get; private set; } = null!;
    public StockName StockName { get; private set; } = null!;
    public StockPrice SellingPrice { get; private set; } = null!;
    public StockPrice BuyingPrice { get; private set; } = null!;
    
    public static StocksModel Create(Name name, Email email, StockName stockName, StockPrice sellingPrice, StockPrice buyingPrice)
    {
        var stock = new StocksModel
        {
            SubscriberName = name,
            SubscriberEmail = email,
            StockName = stockName,
            SellingPrice = sellingPrice,
            BuyingPrice = buyingPrice
        };
        return stock;
    }
}