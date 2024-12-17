namespace StocksMonitorService.Stocks.Services;

public class DailyStocksService : IStockService<StockDailyDataResponse, DailyData>
{
    private readonly StocksServiceUtilities _utilities;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;
    private readonly ILogger<IntradayStocksService> _logger;
    
    public DailyStocksService(StocksServiceUtilities utilities, IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<IntradayStocksService> logger)
    {
        _utilities = utilities;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _apiKey = configuration.GetValue<string>("AlphaVantage:ApiKey") ?? throw new ArgumentNullException(paramName: nameof(_apiKey));
    }
    
    public async Task<KeyValuePair<string, DailyData>?> GetMostRecentStockDataAsync(string stockName)
    {
        var historicalData = await GetHistoricalStockDataAsync(stockName);
        var mostRecentData = historicalData?.TimeSeries?.FirstOrDefault();
        if (mostRecentData == null) return null;
        // var stockDate = DateTime.Parse(mostRecentData.Value.Key);
        // if (!HasStockDataBeenUpdated(stockDate))
        // {
        //     _logger.LogInformation($"[STOCKS-MONITOR] The data has not been updated yet.");
        //     return null;
        // }
        return mostRecentData;
    }

    public async Task<StockDailyDataResponse?> GetHistoricalStockDataAsync(string stockName)
    {
        var data = await GetStockDataResponseFromTxt();
        return data;
    }

    public bool HasStockDataBeenUpdated(DateTime date)
    {
        throw new NotImplementedException();
    }
    
    private async Task<StockDailyDataResponse?> GetStockDataResponseFromTxt()
    {
        const string filePath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksMonitor/StocksMonitorService/MockData/DailyStockDataResponse.txt";
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var stockResponse = JsonSerializer.Deserialize<StockDailyDataResponse>(jsonContent);
        return stockResponse;
    } 
}