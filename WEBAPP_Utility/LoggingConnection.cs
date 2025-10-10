using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace WebApp_Utility
{
    public class MyConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<MyConnectionHandler> _logger;

        public MyConnectionHandler(ILogger<MyConnectionHandler> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            _logger.LogInformation("=========> TCP connected: {Id} from {Remote} to {Local}", 
                connection.ConnectionId, connection.RemoteEndPoint, connection.LocalEndPoint);

            try
            {
                // ждём закрытия соединения
                await Task.Delay(Timeout.Infinite, connection.ConnectionClosed);
            }
            catch (TaskCanceledException)
            {
                // соединение закрылось
            }

            _logger.LogInformation("=========> TCP disconnected: {Id}", connection.ConnectionId);
        }
    }
}
