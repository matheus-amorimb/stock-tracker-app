namespace StocksMonitorService.Stocks.Types.AlphaVantage;

public interface IStockDataResponse<TData>
{
    MetaData? MetaData { get; set; }
    Dictionary<string, TData>? TimeSeries { get; }
}