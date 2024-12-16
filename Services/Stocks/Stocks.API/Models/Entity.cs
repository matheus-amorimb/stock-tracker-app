namespace Stocks.API.Models;

public abstract class Entity<T> : IEntity<T>
{
    public T? Id { get; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}