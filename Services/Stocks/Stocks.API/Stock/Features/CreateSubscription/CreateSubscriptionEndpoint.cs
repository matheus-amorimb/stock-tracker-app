namespace Stocks.API.Stock.Features.CreateSubscription;

public record CreateSubscriptionRequest(string Name, string Email, string StockName, decimal SellingPrice, decimal BuyingPrice);
public record CreateSubscriptionResponse(bool IsSubscribed);

public class CreateSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("stocks/subscribe", async (CreateSubscriptionRequest request, ISender sender) =>
        {
            var subscriptionDto = request.Adapt<SubscriptionDto>();
            var result = await sender.Send(new CreateSubscriptionCommand(subscriptionDto));
            var response = result.Adapt<CreateSubscriptionResponse>();
            return Results.Created($"stocks/subscribed/{response.IsSubscribed}",response);
        })
            .WithName("Subscribe")
            .Produces<CreateSubscriptionResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}