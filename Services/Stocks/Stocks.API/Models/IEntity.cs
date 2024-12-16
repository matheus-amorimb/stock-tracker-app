namespace Stocks.API.Models;

public interface IEntity<out T> : IEntity
{
    public T? Id { get; }
}

public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
