namespace Stocks.API.Stock.Features.GetSubscriptionsByEmail;

public record GetSubscriptionsByEmailResponse(string SubscriberEmail, IEnumerable<GetSubscriptionsByEmailDto> Subscriptions);

public class GetSubscriptionsByEmailEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/subscription/{email}", async (string email, ISender sender) =>
        {
            var query = new GetSubscriptionsByEmailQuery(email);
            var result = await sender.Send(query);
            var response = new GetSubscriptionsByEmailResponse(email, result.Subscriptions); 
            return Results.Ok(response);
        })
            .WithName("SubscriptionsByEmail")
            .Produces<GetSubscriptionsByEmailResponse>(StatusCodes.Status200OK);
    }
}