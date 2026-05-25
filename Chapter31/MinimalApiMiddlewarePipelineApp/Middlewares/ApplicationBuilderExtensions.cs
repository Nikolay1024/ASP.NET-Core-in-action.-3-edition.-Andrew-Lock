namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        // Создание метода расширения типа IApplicationBuilder для промежуточного ПО HeadersMiddleware.
        // По соглашению, метод должен возвращать IApplicationBuilder, чтобы разрешить построение цепочки.
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<SecurityHeadersMiddleware>();

        public static IApplicationBuilder UseSecurityTxtMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<SecurityTxtMiddleware>();

        public static IApplicationBuilder UsePingPong3Middleware(this IApplicationBuilder app)
        {
            return app.Use(async (HttpContext context, Func<Task> next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ping3"))
                    await context.Response.WriteAsync("pong3");
                else
                    await next();
            });
        }
    }
}
