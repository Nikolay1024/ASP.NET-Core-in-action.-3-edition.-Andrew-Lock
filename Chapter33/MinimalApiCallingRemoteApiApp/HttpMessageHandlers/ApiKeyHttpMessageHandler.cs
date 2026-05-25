using Microsoft.Extensions.Options;
using MinimalApiCallingRemoteApiApp.Options;

namespace MinimalApiCallingRemoteApiApp.HttpMessageHandlers
{
    // Пользовательские обработчики HttpMessageHandler в конвейере HTTP клиента должны наследовать от
    // DelegatingHandler.
    public class ApiKeyHttpMessageHandler : DelegatingHandler
    {
        private readonly RemoteApiOptions _options;

        // Внедрение строго типизированных значений конфигурации.
        public ApiKeyHttpMessageHandler(IOptions<RemoteApiOptions> options) => _options = options.Value;

        // Переопределяем метод SendAsync().
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Добавляем заголовок ко всем исходящим запросам.
            request.Headers.Add("TEST-API-KEY", _options.ApiKey);

            // Метод base.SendAsync() вызывает оставшуюся часть конвейера HTTP клиента и получает ответ.
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
