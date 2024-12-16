namespace Stocks.API.Stock.ValueObjects;

public class Name
{
    public string Value { get;}

    private Name(string subscriberName)
    {
        Value = subscriberName;
    }
    
    public static Name Of(string subscriberName)
    { 
        ArgumentException.ThrowIfNullOrEmpty(subscriberName, nameof(subscriberName));
        return new Name(subscriberName);
    }
}