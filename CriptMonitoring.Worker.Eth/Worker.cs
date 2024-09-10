using System.Timers;
using CriptMonitoring.Application.Interfaces;
using Timer = System.Timers.Timer;

namespace CriptMonitoring.Worker.Eth;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IWebSocketClientService _webSocketClientService;
    private IBitstampService _bitstampService;
    private static Timer _timer;

    public Worker(ILogger<Worker> logger ,IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(5000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;

        
        using var scope = _serviceProvider.CreateScope();
        _webSocketClientService = scope.ServiceProvider.GetService<IWebSocketClientService>();
        _bitstampService =  scope.ServiceProvider.GetService<IBitstampService>();
        
        var uri = new Uri("wss://ws.bitstamp.net"); 

        _logger.LogInformation("Connecting to WebSocket...");
        await _webSocketClientService.ConnectAsync(uri, stoppingToken);
        
        var payload = new
        {
            @event = "bts:subscribe",
            data = new
            {
                channel = "order_book_ethusd"
            }
        };
        
        await _webSocketClientService.SendJsonAsync(payload, stoppingToken);
        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await _webSocketClientService.ReceiveAsync(stoppingToken);
                
            if (i > 0)
            {
                await _bitstampService.ProcessCripto(message ,"ETH");
            }
            i++;
        }
    }

    private void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        var dto = _bitstampService.GetStatics("ETH");
        _logger.LogInformation($"AvgPrice: {dto.AvgPrice} MaxPrice: {dto.MaxPrice} MinPrice: {dto.MinPrice} AvgLastFiveSeconds: {dto.AvgLastFiveSeconds}");
    }
}