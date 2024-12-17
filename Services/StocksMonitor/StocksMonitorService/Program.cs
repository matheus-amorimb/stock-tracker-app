var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddServices(builder.Configuration);

var host = builder.Build();
host.Run();