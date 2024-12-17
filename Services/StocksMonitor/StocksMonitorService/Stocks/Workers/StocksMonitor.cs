namespace StocksMonitorService.Stocks.Workers;

public class StocksMonitor(StocksService stocksService, CacheRepository cacheRepository, IServiceProvider serviceProvider, ILogger<StocksMonitor> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation($"[STOCKS-MONITOR] Monitoring stocks at: {DateTimeOffset.Now}");
        var activeStocks = (await cacheRepository.GetActiveStocks()).ToList();
        if (activeStocks.Count != 0)
        {
            var monitoringTasks = activeStocks.Select(MonitorStocksAsync);
            await Task.WhenAll(monitoringTasks);
        }
    }

    private async Task MonitorStocksAsync(string stockName)
    {
        var stockData = await stocksService.GetMostRecentStockDataAsync(stockName);
        if (stockName != "IBM") return;
        if (stockData == null) return;
        
        var stockDate = DateTime.Parse(stockData.Value.Key);
        // if (!HasStockDataBeenUpdated(stockDate)) return;
        if (HasStockDataBeenUpdated(stockDate)) return;
        
        Decimal.TryParse(stockData.Value.Value.High, out var stockHighPrice);
        Decimal.TryParse(stockData.Value.Value.Low, out var stockLowPrice);
        
        var subscribers = await cacheRepository.GetSubscriptionsForStock(stockName);
        foreach (var subscriber in subscribers)       
        {
            var buyingPrice = subscriber?.BuyingPrice;
            var sellingPrice = subscriber?.SellingPrice;
            
            if (stockHighPrice >= sellingPrice || stockLowPrice <= buyingPrice)
            {
                var priceAlertTriggeredEvent = new PriceAlertTriggeredEvent
                {
                    SubscriberName = subscriber?.SubscriberName,
                    SubscriberEmail = subscriber?.SubscriberEmail,
                    StockName = stockName,
                    SellingPrice = subscriber?.SellingPrice,
                    BuyingPrice = subscriber?.BuyingPrice,
                    StockHighPrice = stockHighPrice,
                    StockLowPrice = stockLowPrice,
                    StockDateTime = stockDate
                };
                
                await PublishEvent(priceAlertTriggeredEvent);
                logger.LogInformation($"[STOCKS-MONITOR] Alert event triggered for subscriber {subscriber?.SubscriberEmail.ToUpperInvariant()} and stock {stockName}");
            }
        }
    }
    
    private bool HasStockDataBeenUpdated(DateTime lastUpdatedDate)
    {
        var dateLimit = lastUpdatedDate.AddMinutes(60);
        return dateLimit >= DateTime.Now;
    }

    private async Task PublishEvent(PriceAlertTriggeredEvent priceAlertTriggeredEvent)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(priceAlertTriggeredEvent);
    }
}