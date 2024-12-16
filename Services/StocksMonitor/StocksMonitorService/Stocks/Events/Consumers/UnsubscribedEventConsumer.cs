namespace StocksMonitorService.Stocks.Events.Consumers;

public class UnsubscribedEventConsumer(CacheRepository cacheRepository) : IConsumer<UnsubscribedEvent>
{
    public async Task Consume(ConsumeContext<UnsubscribedEvent> context)
    {
        var subscribedEvent = context.Message;
        await cacheRepository.RemoveSubscription(subscribedEvent);
    }
}