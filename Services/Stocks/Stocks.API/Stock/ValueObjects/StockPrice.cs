namespace Stocks.API.Stock.ValueObjects;

public class StockPrice
{
    public decimal Value { get;  }

    private StockPrice(decimal value)
    {
        Value = value;
    }
    
    public static StockPrice Of(decimal value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than zero");
        return new StockPrice(value);
    }
}