namespace StocksMonitorService;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDatabase>(_ =>
        {
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") ?? throw new ArgumentNullException());
            return redis.GetDatabase();
        });
        services.AddSingleton<CacheRepository>();
        services.AddSingleton<StocksService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddMessageBroker(configuration);
        return services;
    }
}