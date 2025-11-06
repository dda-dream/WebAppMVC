using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace WebApp_Utility
{
    using Microsoft.AspNetCore.Connections;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class MyConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<MyConnectionHandler> _logger;

        public MyConnectionHandler(ILogger<MyConnectionHandler> logger)
        {
            _logger = logger;
        }





        public override async Task OnConnectedAsync(ConnectionContext connection)
        {

            _logger.LogInformation("TCP connected: {Id} from {Remote} to {Local}", 
                connection.ConnectionId, connection.RemoteEndPoint, connection.LocalEndPoint);

            var input = connection.Transport.Input;

            try
            {

                while (true)
                {
                    // Чтение из потока с поддержкой отмены при закрытии соединения
                    var result = await input.ReadAsync(connection.ConnectionClosed);
                    var buffer = result.Buffer;

                    if (!buffer.IsEmpty)
                    {
                        // Можно обрабатывать данные
                        _logger.LogInformation("Received {Length} bytes from {Id}", buffer.Length, connection.ConnectionId);
                    }

                    // Сообщаем Kestrel, что мы продвинулись по буферу
                    input.AdvanceTo(buffer.End);

                    if (result.IsCompleted)
                        break; // соединение закрыто
                }
            }
            catch (OperationCanceledException)
            {
                // connection.ConnectionClosed сработал, соединение закрыто
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в обработке соединения {Id}", connection.ConnectionId);
            }
            finally
            {
                _logger.LogInformation("TCP disconnected: {Id}", connection.ConnectionId);
            }
        }

    }

}
