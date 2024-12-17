namespace StocksMonitorService.Stocks.Services;

public interface IStockService<T, TData> where T: IStockDataResponse<TData>
{
    Task<KeyValuePair<string, TData>?> GetMostRecentStockDataAsync(string stockName);
    Task<T?> GetHistoricalStockDataAsync(string stockName);
    bool HasStockDataBeenUpdated(DateTime date);
}