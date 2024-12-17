namespace StocksMonitorService.Stocks.Services;

public class StocksServiceUtilities (IHttpClientFactory httpClientFactory, ILogger<StocksServiceUtilities> logger)
{
    public async Task<string?> GetValidSymbolForStock(string stockName, string apiKey)
    {
        Uri uri = new Uri($"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={stockName}&apikey={apiKey}");
        var client = httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetStringAsync(uri);
            var getStockSymbolResponse = JsonSerializer.Deserialize<StockSymbolResponse>(response);
            return getStockSymbolResponse?.BestMatches?.FirstOrDefault()?.Symbol;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
        }
        return null;
    }
}