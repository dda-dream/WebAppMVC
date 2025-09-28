using Microsoft.AspNetCore.Connections;

namespace WebAppMVC
{
    public class LoggingConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<LoggingConnectionHandler> _logger;

        public LoggingConnectionHandler(ILogger<LoggingConnectionHandler> logger)
        {
            //(LoggingConnectionHandler)logger.
            
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
