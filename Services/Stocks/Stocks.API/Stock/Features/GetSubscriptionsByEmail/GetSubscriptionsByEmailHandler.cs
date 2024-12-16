namespace Stocks.API.Stock.Features.GetSubscriptionsByEmail;

public record GetSubscriptionsByEmailQuery(string Email) : IRequest<GetSubscriptionsByEmailResult>; 
public record GetSubscriptionsByEmailResult(IEnumerable<GetSubscriptionsByEmailDto> Subscriptions);

public record GetSubscriptionsByEmailDto
{
    public StockName StockName { get; private set; } = null!;
    public StockPrice SellingPrice { get; private set; } = null!;
    public StockPrice BuyingPrice { get; private set; } = null!;
}

public class GetSubscriptionsByEmailHandler(IRepository<StocksModel> repository) : IRequestHandler<GetSubscriptionsByEmailQuery, GetSubscriptionsByEmailResult>
{
    public async Task<GetSubscriptionsByEmailResult> Handle(GetSubscriptionsByEmailQuery query, CancellationToken cancellationToken)
    {
        var subscriptions = await repository.GetAllAsyncBySubscriberEmail(Email.Of(query.Email), cancellationToken);
        var subscriptionsDto = subscriptions.Select(sub => sub.Adapt<GetSubscriptionsByEmailDto>()); 
        return new GetSubscriptionsByEmailResult(subscriptionsDto);
    }
}