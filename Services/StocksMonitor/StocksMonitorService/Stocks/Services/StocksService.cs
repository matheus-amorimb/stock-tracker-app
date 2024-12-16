namespace StocksMonitorService.Stocks.Services;

public class StocksService (IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<StocksService> logger)
{
    private readonly string _apiKey = config.GetValue<string>("AlphaVantage:ApiKey")?.ToUpperInvariant() ?? throw new ArgumentNullException(paramName: nameof(_apiKey));
    public async Task<GetStockDataResponse?> GetHistoricalStockDataAsync(string stockName)
    {
        // var endpointUrl = await BuildEndpointUrlForStock(stockName);
        // if (endpointUrl == null) return null;
        // var data = await GetStockDataResponseAsync(endpointUrl);
        var data = await GetStockDataResponseFromTxt();
        return data;
    }

    public async Task<KeyValuePair<string, TimeSeriesEntry>?> GetMostRecentStockDataAsync(string stockName)
    {
        var historicalData = await GetHistoricalStockDataAsync(stockName);
        return historicalData?.TimeSeries?.FirstOrDefault();
    }
    
    private async Task<string?> BuildEndpointUrlForStock(string stockName)
    {
        var stockSymbol = await GetValidSymbolForStock(stockName);
        if (stockSymbol == null) return null;
        return $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={stockSymbol}&interval=60min&apikey={_apiKey}";
    }

    private async Task<string?> GetValidSymbolForStock(string stockName)
    {
        Uri uri = new Uri($"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={stockName}&apikey={_apiKey}");
        var client = httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetStringAsync(uri);
            var getStockSymbolResponse = JsonSerializer.Deserialize<GetStockSymbolResponse>(response);
            return getStockSymbolResponse?.BestMatches?.FirstOrDefault()?.Symbol;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
        }

        return null;
    }

    private async Task<GetStockDataResponse?> GetStockDataResponseAsync(string endpointUrl)
    {
        var uri = new Uri(endpointUrl);
        var client = httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetStringAsync(uri);
            var getStockDataResponse = JsonSerializer.Deserialize<GetStockDataResponse>(response);
            return getStockDataResponse;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
        }
        return null;
    }     
    
    private async Task<GetStockDataResponse?> GetStockDataResponseFromTxt()
    {
        const string filePath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksMonitor/StocksMonitorService/StockDataResponse.txt";
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var stockResponse = JsonSerializer.Deserialize<GetStockDataResponse>(jsonContent);
        return stockResponse;
    } 
    
    private async Task<string?> GetValidSymbolForStockFromTxt()
    {
        const string filePath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksMonitor/StocksMonitorService/GetStockSymbolResponse.txt";
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var getStockSymbolResponse = JsonSerializer.Deserialize<GetStockSymbolResponse>(jsonContent);
        return getStockSymbolResponse?.BestMatches?.FirstOrDefault()?.Symbol;
    }
}