using System.Diagnostics;
using System.Reflection;

namespace MinimalApiMiddlewarePipelineApp.Middlewares
{
    public class VersionMiddleware
    {
        static readonly string? _version =
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location)?.FileVersion;

        public VersionMiddleware(RequestDelegate next) { }

        public async Task Invoke(HttpContext context) =>
            await context.Response.WriteAsJsonAsync(new { Version = _version });
    }
}