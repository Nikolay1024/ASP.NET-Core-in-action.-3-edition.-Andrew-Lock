namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    public class CalculatorMiddleware
    {
        public CalculatorMiddleware(RequestDelegate next) { }

        public async Task Invoke(HttpContext context)
        {
            //RouteValueDictionary routeValues = context.GetRouteData().Values;
            //string? aValue = routeValues["a"] as string;
            //string? bValue = routeValues["b"] as string;

            string? aValue = context.GetRouteValue("a") as string;
            string? bValue = context.GetRouteValue("b") as string;

            if (!int.TryParse(aValue, out int a))
            {
                context.Response.StatusCode = 400;
                return;
            }
            if (!int.TryParse(bValue, out int b))
            {
                context.Response.StatusCode = 400;
                return;
            }

            await context.Response.WriteAsync($"{a} + {b} = {a + b}");
        }
    }
}
