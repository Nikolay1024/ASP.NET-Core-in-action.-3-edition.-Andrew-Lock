namespace MinimalApiXunitTestingApp.Middlewares
{
    public class StatusMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/ping"))
            {
                await httpContext.Response.WriteAsync("pong");
                return;
            }
            await _next(httpContext);
        }
    }
}
