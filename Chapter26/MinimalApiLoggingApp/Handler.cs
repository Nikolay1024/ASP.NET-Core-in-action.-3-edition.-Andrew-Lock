namespace MinimalApiLoggingApp
{
    public class Handler
    {
        public static IEnumerable<string> GetValues(ValuesService service, ILogger<Handler> logger)
        {
            logger.LogInformation("Внутри обработчика (handler). Снуружи внешней области (outer scope).");

            using (logger.BeginScope(123))
            using (logger.BeginScope(new Dictionary<string, object> { { "ScopeValue1", "outer scope" } }))
            {
                logger.LogInformation("Внутри обработчика (handler). Внутри внешней области (outer scope).");
                return service.GetValues();
            }
        }
    }
}