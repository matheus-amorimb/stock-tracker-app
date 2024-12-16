namespace Stocks.API;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddMessaging(services, configuration);
        AddRepositories(services);
        AddApplicationServices(services);
        AddSwagger(services);
        AddErrorHandling(services);

        return services;
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("StocksDatabase");
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContext<AppDbContext>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            optionsBuilder
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });
        services.AddScoped<IAppDbContext, AppDbContext>();
    }

    private static void AddMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBroker(configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IRepository<StocksModel>, StocksRepository>();
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddCarter();
        services.AddControllers();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void AddErrorHandling(IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}