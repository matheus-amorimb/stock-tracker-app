namespace Stocks.API.Data;

public interface IAppDbContext
{
    DbSet<StocksModel> Stocks { get;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}