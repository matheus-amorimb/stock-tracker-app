namespace StocksNotification.MessageBroker;

public static class BrokerExtension
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        var hostUri = new Uri(configuration.GetValue<string>("MessageBroker:Host") ?? throw new NullReferenceException());
        var userName = configuration.GetValue<string>("MessageBroker:UserName") ?? throw new NullReferenceException();
        var password = configuration.GetValue<string>("MessageBroker:Password") ?? throw new NullReferenceException();
        
        var priceAlertTopicName = configuration.GetValue<string>("MessageBroker:PriceAlertTopic") ?? throw new NullReferenceException();
        var priceAlertQueueName = configuration.GetValue<string>("MessageBroker:PriceAlertQueue") ?? throw new NullReferenceException();
        
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.AddConsumer<PriceAlertTriggeredConsumer>();
            
            cfg.UsingRabbitMq(((context, configurator) =>
            {
                configurator.Host(hostUri, hostConfigurator =>
                {
                    hostConfigurator.Username(userName);
                    hostConfigurator.Password(password);
                });
                
                configurator.ReceiveEndpoint(priceAlertQueueName, endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumeTopology = false;
                    endpointConfigurator.ConfigureConsumer<PriceAlertTriggeredConsumer>(context);
                    endpointConfigurator.Bind(priceAlertTopicName);
                });                
            }));
        });
        return services;
    }
}