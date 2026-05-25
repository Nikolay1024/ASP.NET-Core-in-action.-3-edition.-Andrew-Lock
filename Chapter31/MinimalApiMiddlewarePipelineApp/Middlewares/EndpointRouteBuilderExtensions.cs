namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    public static class EndpointRouteBuilderExtensions
    {
        // Создаем метод расширения IEndpointRouteBuilder для регистрации промежуточного ПО PingPongMiddleware в
        // качестве конечной точки.
        public static IEndpointConventionBuilder MapPingPong4(this IEndpointRouteBuilder endpoints, string pattern)
        {
            // Создаем конвейер промежуточного ПО для обработки конечной точки.
            RequestDelegate pipline = endpoints.CreateApplicationBuilder()
                // Добавляем промежуточное ПО в конвейер.
                .UseMiddleware<PingPong4Middleware>()
                // Собираем конвейер из добавленного промежуточного ПО.
                .Build();

            // Создаем новую конечную точку, сопоставляя с шаблоном маршрута pattern конвейер промежуточного ПО.
            // Вы можете добавить метаданные в методе расширения и/или в файле Program.cs.
            return endpoints.Map(pattern, pipline).WithName("PingPong4");
        }

        public static IEndpointConventionBuilder MapVersion(this IEndpointRouteBuilder endpoints, string pattern)
        {
            RequestDelegate pipline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<VersionMiddleware>()
                .Build();

            return endpoints.Map(pattern, pipline);
        }

        public static IEndpointConventionBuilder MapMiddlewareAsEndpoint<T>(this IEndpointRouteBuilder endpoints,
            string pattern)
        {
            RequestDelegate pipline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<T>()
                .Build();

            return endpoints.Map(pattern, pipline);
        }
    }
}
