namespace Stocks.API.Stock.Features.RemoveSubscription;

public record RemoveSubscriptionResponse(bool IsUnsubscribed);

public class RemoveSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/stocks/unsubscribe/{email}/{stockName}", async (string email, string stockName, ISender sender) =>
        {
            var command = new RemoveSubscriptionCommand(email, stockName);
            var result = await sender.Send(command);
            var response = new RemoveSubscriptionResponse(result.IsUnsubscribed);
            return Results.Ok(response);
        })
            .WithName("Unsubscribe")
            .Produces<RemoveSubscriptionResponse>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
    }
}