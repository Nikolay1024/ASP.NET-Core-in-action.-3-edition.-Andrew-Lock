using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MinimalApiCallingRemoteApiApp.Options;
using System.Net.Mime;

namespace MinimalApiCallingRemoteApiApp.HttpClients
{
    // Типизированный HTTP клиент для тестового удаленного API JsonPlaceholder.
    public class RemoteApiHttpClient
    {
        private readonly HttpClient _httpClient;

        // Внедрение сервиса HttpClient вместо IHttpClientFactory.
        public RemoteApiHttpClient(HttpClient httpClient, IOptions<RemoteApiOptions> options)
        {
            _httpClient = httpClient;

            // Можно конфигурировать типизированный HTTP клиент либо в файле Program.cs, либо в конструкторе
            // типизированного HTTP клиента.
            //_httpClient.BaseAddress = new Uri(options.Value.Url);
        }

        // Метод GetAlbum4Async() инкапсулирует логику взаимодействия с удаленным API.
        public async Task<FileStreamHttpResult> GetAlbum4Async()
        {
            HttpResponseMessage hrm = await _httpClient.GetAsync("albums/4");
            hrm.EnsureSuccessStatusCode();

            Stream stream = await hrm.Content.ReadAsStreamAsync();
            FileStreamHttpResult fshr = TypedResults.Stream(stream, MediaTypeNames.Application.Json);
            return fshr;
        }
    }
}
