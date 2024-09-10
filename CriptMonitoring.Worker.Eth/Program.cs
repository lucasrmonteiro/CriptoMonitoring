using CriptMonitoring.Application.Interfaces;
using CriptMonitoring.Application.Services;
using CriptMonitoring.Data.Interfaces;
using CriptMonitoring.Data.Repository;
using CriptMonitoring.Infra.Config;
using CriptMonitoring.Worker.Eth;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddSingleton<IWebSocketClientService, WebSocketClientService>();

var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IBitstampRepository, BitstampRepository>(it =>
{
    return new BitstampRepository(it.GetRequiredService<IMongoClient>(), settings.DatabaseName, settings.CollectionName);
});
builder.Services.AddTransient<IBitstampService, BitstampService>();


var host = builder.Build();
host.Run();