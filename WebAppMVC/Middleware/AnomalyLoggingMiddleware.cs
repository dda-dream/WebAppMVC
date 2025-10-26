namespace WebAppMVC.Middleware
{
    public class AnomalyLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AnomalyLoggingMiddleware> _logger;

        public AnomalyLoggingMiddleware(RequestDelegate next, ILogger<AnomalyLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            //подозрительный метод
            if (request.Method != "GET" && request.Method != "POST" &&
                request.Method != "PUT" && request.Method != "DELETE")
            {
                _logger.LogWarning("🚨 Подозрительный HTTP-метод: {Method} от {IP}", 
                    request.Method, context.Connection.RemoteIpAddress);
            }
            //слишком длинный заголовок Host
            if (request.Headers.Host.Count > 0 && request.Headers.Host[0].Length > 100)
            {
                _logger.LogWarning("🚨 Аномальный Host-заголовок ({Length} символов) от {IP}", 
                    request.Headers.Host[0].Length, context.Connection.RemoteIpAddress);
            }

            // Можно добавить проверки размера тела, query, user-agent и т.п.

            await _next(context);
        }
    }

}
