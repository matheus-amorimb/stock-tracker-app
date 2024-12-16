namespace Stocks.API.Stock.Data;

public class StocksConfiguration : IEntityTypeConfiguration<Models.StocksModel>
{
    public void Configure(EntityTypeBuilder<Models.StocksModel> builder)
    {
        builder.HasKey(stocks => stocks.Id);
        
        builder.HasIndex(model => new { model.SubscriberEmail, model.StockName }).IsUnique();
        
        builder.Property(s => s.SubscriberName)
            .HasConversion(
                name => name.Value, 
                dbName => Name.Of(dbName));
                
        builder.Property(s => s.SubscriberEmail)
            .HasConversion(
                email => email.Value, 
                dbEmail => Email.Of(dbEmail));        
        
        builder.Property(s => s.StockName)
            .HasConversion(
                stockName => stockName.Value, 
                dbStockName => StockName.Of(dbStockName));
        
        builder.Property(s => s.SellingPrice)
            .HasConversion(
                stockPrice => stockPrice.Value, 
                dbStockPrice => StockPrice.Of(dbStockPrice));
        
        builder.Property(s => s.BuyingPrice)
            .HasConversion(
                stockPrice => stockPrice.Value, 
                dbStockPrice => StockPrice.Of(dbStockPrice));
    }
}