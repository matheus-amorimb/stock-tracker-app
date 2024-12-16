namespace StocksMonitorService.MessageBroker;

public static class BrokerExtension
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        var hostUri = new Uri(configuration.GetValue<string>("MessageBroker:Host") ?? throw new NullReferenceException());
        var userName = configuration.GetValue<string>("MessageBroker:UserName") ?? throw new NullReferenceException();
        var password = configuration.GetValue<string>("MessageBroker:Password") ?? throw new NullReferenceException();
        
        var subscribedTopicName = configuration.GetValue<string>("MessageBroker:SubscribedTopic") ?? throw new NullReferenceException();
        var subscribedEventQueueName = configuration.GetValue<string>("MessageBroker:SubscribedEventQueue") ?? throw new NullReferenceException();
        
        var unsubscribedTopicName = configuration.GetValue<string>("MessageBroker:UnsubscribedTopic") ?? throw new NullReferenceException();
        var unsubscribedEventQueueName = configuration.GetValue<string>("MessageBroker:UnsubscribedEventQueue") ?? throw new NullReferenceException();
        
        var priceAlertTriggeredTopicName = configuration.GetValue<string>("MessageBroker:PriceAlertTriggeredTopic") ?? throw new NullReferenceException();
        
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.AddConsumer<SubscribedEventConsumer>();
            cfg.AddConsumer<UnsubscribedEventConsumer>();
            
            cfg.UsingRabbitMq(((context, configurator) =>
            {
                configurator.Host(hostUri, hostConfigurator =>
                {
                    hostConfigurator.Username(userName);
                    hostConfigurator.Password(password);
                });
                
                configurator.Message<PriceAlertTriggeredEvent>(topologyConfigurator =>
                {
                    topologyConfigurator.SetEntityName(priceAlertTriggeredTopicName);
                });
                
                configurator.ReceiveEndpoint(subscribedEventQueueName, endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumeTopology = false;
                    endpointConfigurator.ConfigureConsumer<SubscribedEventConsumer>(context);
                    endpointConfigurator.Bind(subscribedTopicName);
                });                
                
                configurator.ReceiveEndpoint(unsubscribedEventQueueName, endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumeTopology = false;
                    endpointConfigurator.ConfigureConsumer<UnsubscribedEventConsumer>(context);
                    endpointConfigurator.Bind(unsubscribedTopicName);
                });
            }));
        });
        return services;
    }
}