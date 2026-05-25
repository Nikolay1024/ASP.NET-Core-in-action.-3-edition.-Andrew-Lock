namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    // Обработчик Security.txt, реализованный как промежуточное ПО.
    public class SecurityTxtMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityTxtMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/security.txt"))
                await context.Response.WriteAsync("Contact: mailto:security@example.com");
            else
                await _next(context);
        }
    }
}
