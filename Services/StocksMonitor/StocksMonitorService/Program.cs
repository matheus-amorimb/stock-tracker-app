using StocksMonitorService;
using StocksMonitorService.Stocks.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<StocksMonitor>();
builder.Services.AddServices(builder.Configuration);

var host = builder.Build();
host.Run();