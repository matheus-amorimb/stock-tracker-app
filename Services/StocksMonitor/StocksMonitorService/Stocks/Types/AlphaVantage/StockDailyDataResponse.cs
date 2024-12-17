namespace StocksMonitorService.Stocks.Types.AlphaVantage;

public class StockDailyDataResponse : IStockDataResponse<DailyData>
{
    [JsonPropertyName("Meta Data")]
    public MetaData? MetaData { get; set; }

    [JsonPropertyName("Time Series (Daily)")]
    public Dictionary<string, DailyData>? TimeSeries { get; set; }
}

public class MetaDataDaily
{
    [JsonPropertyName("1. Information")]
    public string? Information { get; set; }

    [JsonPropertyName("2. Symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("3. Last Refreshed")]
    public string? LastRefreshed { get; set; }

    [JsonPropertyName("4. Output Size")]
    public string? OutputSize { get; set; }

    [JsonPropertyName("5. Time Zone")]
    public string? TimeZone { get; set; }
}

public class DailyData
{
    [JsonPropertyName("1. open")]
    public string? Open { get; set; }

    [JsonPropertyName("2. high")]
    public string? High { get; set; }

    [JsonPropertyName("3. low")]
    public string? Low { get; set; }

    [JsonPropertyName("4. close")]
    public string? Close { get; set; }

    [JsonPropertyName("5. volume")]
    public string? Volume { get; set; }
}