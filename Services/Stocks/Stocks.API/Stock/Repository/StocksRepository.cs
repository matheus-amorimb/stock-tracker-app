namespace Stocks.API.Stock.Repository;

public class StocksRepository(IAppDbContext dbContext) : IRepository<StocksModel>
{
    public async Task<StocksModel> AddAsync(StocksModel entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Stocks.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public Task<StocksModel> UpdateAsync(StocksModel entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Email subscriberEmail, StockName stockName, CancellationToken cancellationToken = default)
    {
        var subscription = await GetByEmailAndStockAsync(subscriberEmail, stockName, cancellationToken);
        dbContext.Stocks.Remove(subscription);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<StocksModel>> GetAllAsyncBySubscriberEmail(Email subscriberEmail, CancellationToken cancellationToken)
    {
        var stocks = await dbContext.Stocks.Where(model => model.SubscriberEmail == subscriberEmail)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        if (stocks == null)
        {
            throw new ResourceNotFoundException($"No stocks were found associated with the subscriber email: '{subscriberEmail}'.");
        }
        return stocks;
    }

    public async Task<StocksModel> GetByEmailAndStockAsync(Email subscriberEmail, StockName stockName, CancellationToken cancellationToken)
    {
        var subscription = await dbContext.Stocks.FirstOrDefaultAsync(model => model.SubscriberEmail == subscriberEmail && model.StockName == stockName, cancellationToken: cancellationToken);
        if (subscription == null)
        {
            throw new ResourceNotFoundException($"No subscription found for stock '{stockName.Value}' associated with the subscriber email: '{subscriberEmail.Value}'.");
        }
        return subscription;
    }
}