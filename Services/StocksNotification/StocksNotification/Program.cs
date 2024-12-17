var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<EmailService>();
builder.Services.AddMessageBroker(builder.Configuration);

var host = builder.Build();
host.Run();