namespace Stocks.API.Stock.Features.RemoveSubscription;

public record RemoveSubscriptionCommand(string SubscriberEmail, string StockName) : IRequest<RemoveSubscriptionResult>;
public record RemoveSubscriptionResult(bool IsUnsubscribed);

public class RemoveSubscriptionHandler(IRepository<StocksModel> repository, IMediator mediator) : IRequestHandler<RemoveSubscriptionCommand, RemoveSubscriptionResult>
{
    public async Task<RemoveSubscriptionResult> Handle(RemoveSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var email = Email.Of(command.SubscriberEmail);
        var stockName = StockName.Of(command.StockName);
        var subscription = await repository.GetByEmailAndStockAsync(email, stockName, cancellationToken);
        var unsubscribedEvent = GetUnsubscribedEventFromSubscription(subscription); 
        await repository.DeleteAsync(email, stockName, cancellationToken);
        await mediator.Publish(unsubscribedEvent, cancellationToken);
        return new RemoveSubscriptionResult(true);
    }
    
    private UnsubscribedEvent GetUnsubscribedEventFromSubscription(StocksModel subscription)
    {
        return new UnsubscribedEvent
        {
            SubscriberName = subscription.SubscriberName.Value,
            SubscriberEmail = subscription.SubscriberEmail.Value,
            StockName = subscription.StockName.Value,
            SellingPrice = subscription.SellingPrice.Value,
            BuyingPrice = subscription.BuyingPrice.Value,
        };
    }
}