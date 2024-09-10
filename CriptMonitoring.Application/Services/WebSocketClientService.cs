using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using CriptMonitoring.Application.Interfaces;

namespace CriptMonitoring.Application.Services;

public class WebSocketClientService : IWebSocketClientService
{
    private readonly ClientWebSocket _clientWebSocket;

    public WebSocketClientService()
    {
        _clientWebSocket = new ClientWebSocket();
    }

    public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
    {
        await _clientWebSocket.ConnectAsync(uri, cancellationToken);
    }

    public async Task SendAsync(string message, CancellationToken cancellationToken)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<byte>(bytes);
        await _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
    }

    public async Task SendJsonAsync<T>(T data, CancellationToken cancellationToken)
    {
        string json = JsonSerializer.Serialize(data);
        await SendAsync(json, cancellationToken);
    }

    public async Task<string> ReceiveAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];
        var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
        var message = new StringBuilder();

        while (!result.EndOfMessage)
        {
            message.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
            
            result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
        }
        
        message.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

        if (result.MessageType == WebSocketMessageType.Close)
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
            return null;
        }

        return message.ToString();
    }

    public async Task CloseAsync(CancellationToken cancellationToken)
    {
        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
        _clientWebSocket.Dispose();
    }
}