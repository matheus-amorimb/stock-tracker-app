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
        services.AddScheduler(configuration);
        return services;
    }

    private static IServiceCollection AddScheduler(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("StocksMonitorJob");
            q.AddJob<StocksMonitor>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("StocksMonitorTrigger")
                .WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromMinutes(60))
                    .RepeatForever()));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }
}