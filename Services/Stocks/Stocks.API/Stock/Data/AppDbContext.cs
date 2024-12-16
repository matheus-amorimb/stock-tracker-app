namespace Stocks.API.Stock.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options), IAppDbContext
{
    public DbSet<Models.StocksModel> Stocks => Set<StocksModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}