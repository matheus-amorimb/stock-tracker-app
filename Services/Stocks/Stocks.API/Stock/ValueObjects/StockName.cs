namespace Stocks.API.Stock.ValueObjects;

public class StockName
{
    public string Value { get;}

    private StockName(string stockName)
    {
        Value = stockName;
    }
    
    public static StockName Of(string stockName)
    { 
        ArgumentException.ThrowIfNullOrEmpty(stockName, nameof(stockName));
        return new StockName(stockName);
    }
}