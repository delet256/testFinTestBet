using System.Text;
using TestTask.Models;
using TestTask.Repositories.Interfaces;

namespace TestTask.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LoggingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var loggingRepository = scope.ServiceProvider.GetRequiredService<ILoggingRepository>();

                // Логируем запрос
                var logEntry = await FormatRequest(context.Request);

                // Копируем оригинальный поток ответа, чтобы можно было прочитать его содержимое
                var originalBodyStream = context.Response.Body;

                using var responseBody = new MemoryStream();

                context.Response.Body = responseBody;

                // Продолжаем выполнение pipeline
                await _next(context);

                // Логируем ответ
                await FormatResponse(context.Response, logEntry);

                // Логируем запрос и ответ в базу данных
                await loggingRepository.AddAsync(logEntry);

                // Копируем содержимое ответа обратно в оригинальный поток
                await responseBody.CopyToAsync(originalBodyStream);

            }
        }

        private async Task<LogEntryModel> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;

            return new LogEntryModel
            {
                Body = bodyAsText,
                Url = $"{request.Scheme} {request.Host}{request.Path}",
                Method = request.Method,
                Query = request.QueryString.ToString()
            };
        }

        private async Task FormatResponse(HttpResponse response, LogEntryModel logEntry)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            logEntry.StatusCode = response.StatusCode;
            logEntry.Response = text;
        }
    }

}
