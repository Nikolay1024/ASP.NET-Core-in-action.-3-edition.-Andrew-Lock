namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    // Промежуточное ПО, используемое для преобразования в конечную точку с целью получения преимуществ
    // инфраструктуры ASP.NET Core таких как маршрутизация и авторизация.
    public class PingPong4Middleware
    {
        // Вы должны добавить RequestDelegate в конструктор, даже если он не используется.
        public PingPong4Middleware(RequestDelegate next) { }

        public async Task Invoke(HttpContext context) => await context.Response.WriteAsync("pong4");
    }
}
