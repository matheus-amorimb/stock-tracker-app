namespace Stocks.API.Repositories;

public interface IRepository<T> where T : IEntity
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Email subscriberEmail, StockName stockName, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsyncBySubscriberEmail(Email subscriberEmail, CancellationToken cancellationToken);
    Task<T> GetByEmailAndStockAsync(Email email, StockName stockName, CancellationToken cancellationToken);
}