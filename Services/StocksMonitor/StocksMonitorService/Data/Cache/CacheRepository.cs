namespace StocksMonitorService.Data.Cache;

public class CacheRepository (IDatabase cacheDatabase) : IRepository
{
    public async Task AddToActiveStocks(string stockName)
    {
        await cacheDatabase.SetAddAsync("active_stocks", stockName);
    }

    public async Task RemoveFromActiveStocks(string stockName)
    {   
        await cacheDatabase.SetRemoveAsync("active_stocks", stockName);
    }
    
    public async Task<IEnumerable<string>> GetActiveStocks()
    {
        var activeStocks = await cacheDatabase.SetMembersAsync("active_stocks");
        return activeStocks.Select(stock => stock.ToString()).ToList();
    }

    public async Task AddSubscription(SubscribedEvent subscribedEvent)
    {
        var stockName = subscribedEvent.StockName;
        var serializedSubscription = JsonSerializer.Serialize(subscribedEvent);
        await cacheDatabase.SetAddAsync(stockName, serializedSubscription);
    }    
    
    public async Task RemoveSubscription(UnsubscribedEvent unsubscribedEvent)
    {
        var stockName = unsubscribedEvent.StockName;
        var subscriptionToBeRemoved = await this.GetSubscription(stockName, unsubscribedEvent.SubscriberEmail);
        var serializedSubscription = JsonSerializer.Serialize(subscriptionToBeRemoved);
        await cacheDatabase.SetRemoveAsync(stockName, serializedSubscription);
        var subscriptionsToStockCount = await cacheDatabase.SetLengthAsync(stockName);
        if (subscriptionsToStockCount == 0)
        {
            await RemoveFromActiveStocks(stockName);
        }
    }

    public async Task<IEnumerable<SubscribedEvent?>> GetSubscriptionsForStock(string stockName)
    {
        var subscriptions = await cacheDatabase.SetMembersAsync(stockName);
        return subscriptions.Select(stock => JsonSerializer.Deserialize<SubscribedEvent>(stock));
    }
    
    private async Task<SubscribedEvent?> GetSubscription(string stockName, string subscriberEmail)
    {
        var allSubscriptions = await GetSubscriptionsForStock(stockName);
        var subscription = allSubscriptions.FirstOrDefault(s => s.SubscriberEmail == subscriberEmail);
        return subscription;
    }
}