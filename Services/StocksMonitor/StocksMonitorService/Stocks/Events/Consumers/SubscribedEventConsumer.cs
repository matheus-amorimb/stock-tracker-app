namespace StocksMonitorService.Stocks.Events.Consumers;

public class SubscribedEventConsumer(CacheRepository cacheRepository) : IConsumer<SubscribedEvent>
{
    public async Task Consume(ConsumeContext<SubscribedEvent> context)
    {
        var subscribedEvent = context.Message;
        var stockName = subscribedEvent.StockName;
        await cacheRepository.AddToActiveStocks(stockName);
        await cacheRepository.AddSubscription(subscribedEvent);
    }
}