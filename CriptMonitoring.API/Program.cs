using CriptMonitoring.Application.Dto;
using CriptMonitoring.Application.Interfaces;
using CriptMonitoring.Application.Services;
using CriptMonitoring.Data.Interfaces;
using CriptMonitoring.Data.Repository;
using CriptMonitoring.Infra.Config;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
builder.Services.AddTransient<IOperationService, OperationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/operation", async (IOperationService operationService, OperationRequestDto operationRequestDto) =>
    {
        return operationService.DoOperation(operationRequestDto);
    })
    .WithName("save")
    .WithOpenApi();

app.Run();
