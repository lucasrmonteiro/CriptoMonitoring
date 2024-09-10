namespace CriptMonitoring.Application.Interfaces;

public interface IWebSocketClientService
{
    Task ConnectAsync(Uri uri, CancellationToken cancellationToken);
    Task SendAsync(string message, CancellationToken cancellationToken);
    Task SendJsonAsync<T>(T data, CancellationToken cancellationToken);
    Task<string> ReceiveAsync(CancellationToken cancellationToken);
    Task CloseAsync(CancellationToken cancellationToken);
}