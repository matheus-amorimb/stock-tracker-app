namespace StocksMonitorService.Data;

public interface IRepository
{
    Task AddToActiveStocks(string stockName);
    Task RemoveFromActiveStocks(string stockName);
    Task<IEnumerable<string>> GetActiveStocks();
    Task AddSubscription(SubscribedEvent subscribedEvent);
    Task RemoveSubscription(UnsubscribedEvent unsubscribedEvent);
    Task<IEnumerable<SubscribedEvent?>> GetSubscriptionsForStock(string stockName);
}