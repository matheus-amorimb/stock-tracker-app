namespace StocksMonitorService.Stocks.Services;

public class IntradayStocksService : IStockService<StockIntradayDataResponse, IntradayData>
{
    private readonly StocksServiceUtilities _utilities;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;
    private readonly ILogger<IntradayStocksService> _logger;
    
    public IntradayStocksService(StocksServiceUtilities utilities, IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<IntradayStocksService> logger)
    {
        _utilities = utilities;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _apiKey = configuration.GetValue<string>("AlphaVantage:ApiKey") ?? throw new ArgumentNullException(paramName: nameof(_apiKey));
    }

    public async Task<KeyValuePair<string, IntradayData>?> GetMostRecentStockDataAsync(string stockName)
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
    
    public async Task<StockIntradayDataResponse?> GetHistoricalStockDataAsync(string stockName)
    {
        // var endpointUrl = await BuildEndpointUrlForStock(stockName);
        // if (endpointUrl == null) return null;
        // var data = await GetStockDataResponseAsync(endpointUrl);
        var data = await GetStockDataResponseFromTxt();
        return data;
    }
    
    private async Task<string?> BuildEndpointUrlForStock(string stockName)
    {
        var stockSymbol = await _utilities.GetValidSymbolForStock(stockName, _apiKey);
        if (stockSymbol == null) return null;
        return $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={stockSymbol}&interval=60min&apikey={_apiKey}";
    }
    
    private async Task<StockIntradayDataResponse?> GetStockDataResponseAsync(string endpointUrl)
    {
        var uri = new Uri(endpointUrl);
        var client = _httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetStringAsync(uri);
            var getStockDataResponse = JsonSerializer.Deserialize<StockIntradayDataResponse>(response);
            return getStockDataResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
        }
        return null;
    } 
    
    public bool HasStockDataBeenUpdated(DateTime lastUpdatedDate)
    {
        var dateLimit = lastUpdatedDate.AddMinutes(60);
        return dateLimit >= DateTime.Now;
    }
    
    private async Task<StockIntradayDataResponse?> GetStockDataResponseFromTxt()
    {
        const string filePath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksMonitor/StocksMonitorService/MockData/StockDataResponse.txt";
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var stockResponse = JsonSerializer.Deserialize<StockIntradayDataResponse>(jsonContent);
        return stockResponse;
    } 
    private async Task<string?> GetValidSymbolForStockFromTxt()
    {
        const string filePath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksMonitor/StocksMonitorService/MockData/GetStockSymbolResponse.txt";
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var getStockSymbolResponse = JsonSerializer.Deserialize<StockSymbolResponse>(jsonContent);
        return getStockSymbolResponse?.BestMatches?.FirstOrDefault()?.Symbol;
    }
}