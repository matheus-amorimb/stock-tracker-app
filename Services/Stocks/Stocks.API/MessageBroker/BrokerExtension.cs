namespace Stocks.API.MessageBroker;

public static class BrokerExtension
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq(((context, configurator) =>
            {
                var hostUri = new Uri(configuration.GetValue<string>("MessageBroker:Host") ?? throw new NullReferenceException());
                var userName = configuration.GetValue<string>("MessageBroker:UserName") ?? throw new NullReferenceException();
                var password = configuration.GetValue<string>("MessageBroker:Password") ?? throw new NullReferenceException();
                var subscribedTopicName = configuration.GetValue<string>("MessageBroker:SubscribedTopic") ?? throw new NullReferenceException();
                var unsubscribedTopicName = configuration.GetValue<string>("MessageBroker:UnsubscribedTopic") ?? throw new NullReferenceException();
                var subscribedEventQueueName = configuration.GetValue<string>("MessageBroker:SubscribedEventQueue") ?? throw new NullReferenceException();
                
                configurator.Host(hostUri, hostConfigurator =>
                {
                    hostConfigurator.Username(userName);
                    hostConfigurator.Password(password);
                });
                
                configurator.Message<SubscribedEvent>(topologyConfigurator =>
                {
                    topologyConfigurator.SetEntityName(subscribedTopicName);
                });
                
                configurator.Message<UnsubscribedEvent>(topologyConfigurator =>
                {
                    topologyConfigurator.SetEntityName(unsubscribedTopicName);
                });
            }));
        });
        return services;
    }
}