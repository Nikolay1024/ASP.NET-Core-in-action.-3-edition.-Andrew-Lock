namespace MinimalApiLoggingApp
{
    public class ValuesService
    {
        private readonly ILogger<ValuesService> _logger;

        public ValuesService(ILogger<ValuesService> logger) => _logger = logger;

        public IEnumerable<string> GetValues()
        {
            _logger.LogInformation("Внутри сервиса (service). Снуружи внутренней области (inner scope).");

            using (_logger.BeginScope(456))
            using (_logger.BeginScope(new Dictionary<string, object> { { "ScopeValue2", "inner scope" } }))
            {
                _logger.LogInformation("Внутри сервиса (service). Внутри внутренней области (inner scope).");
                return new string[] { "value1", "value2" };
            }
        }
    }
}