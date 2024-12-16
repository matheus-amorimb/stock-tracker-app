namespace Stocks.API.Stock.Features.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionDto SubscriptionDto) : IRequest<CreateSubscriptionResult>;
public record CreateSubscriptionResult(bool IsSubscribed);
    
public class CreateSubscriptionHandler(IRepository<StocksModel> repository, IMediator mediator) : IRequestHandler<CreateSubscriptionCommand, CreateSubscriptionResult>
{
    public async Task<CreateSubscriptionResult> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = CreateSubscription(command.SubscriptionDto);
        await repository.AddAsync(subscription, cancellationToken);
        var subscribedEvent = GetSubscribedEventFromSubscription(subscription);
        await mediator.Publish(subscribedEvent, cancellationToken);
        return new CreateSubscriptionResult(true);
    }

    private StocksModel CreateSubscription(SubscriptionDto subscriptionDto)
    {
        var subscriberName = Name.Of(subscriptionDto.Name);
        var subscriberEmail = Email.Of(subscriptionDto.Email);
        var stockName = StockName.Of(subscriptionDto.StockName);
        var sellingPrice = StockPrice.Of(subscriptionDto.SellingPrice);
        var buyingPrice = StockPrice.Of(subscriptionDto.BuyingPrice);
        
        return StocksModel.Create(subscriberName, subscriberEmail, stockName, sellingPrice, buyingPrice);
    }

    private SubscribedEvent GetSubscribedEventFromSubscription(StocksModel subscription)
    {
        return new SubscribedEvent
        {
            SubscriberName = subscription.SubscriberName.Value,
            SubscriberEmail = subscription.SubscriberEmail.Value,
            StockName = subscription.StockName.Value,
            SellingPrice = subscription.SellingPrice.Value,
            BuyingPrice = subscription.BuyingPrice.Value,
        };
    }
}