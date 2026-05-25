namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        // RequestDelegate представляет остальную часть конвейера промежуточного ПО.
        private readonly RequestDelegate _next;

        // Вы можете внедрить в конструктор сервисы с жизненным циклом Singlton.
        public SecurityHeadersMiddleware(RequestDelegate next) => _next = next;

        // Метод Invoke() вызывается с HttpContext при получении запроса.
        // Вы можете внедрить в метод Invoke() сервисы с любым жизненным циклом.
        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                // Добавляет заголовок в ответ для дополнительной защиты от XSS атак.
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                // Лямбда-выражение, переданное в метод OnStarting(), должно возвращать Task.
                return Task.CompletedTask;
            });

            // Вызывает следующее промежуточное ПО в конвейере.
            await _next(context);
        }
    }
}
