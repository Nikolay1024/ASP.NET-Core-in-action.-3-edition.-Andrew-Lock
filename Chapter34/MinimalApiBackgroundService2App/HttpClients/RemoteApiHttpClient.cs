using MinimalApiBackgroundService2App.Data;
using System.Text.Json;

namespace MinimalApiBackgroundService2App.HttpClients
{
    public class RemoteApiHttpClient
    {
        private readonly HttpClient _httpClient;

        public RemoteApiHttpClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<List<Album>> GetAlbumsAsync()
        {
            HttpResponseMessage hrm = await _httpClient.GetAsync("albums");
            hrm.EnsureSuccessStatusCode();

            Stream stream = await hrm.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            List<Album> albums = (await JsonSerializer.DeserializeAsync<List<Album>>(stream, options))!;

            // Короткий вариант без проверки статусного кода и потокового чтения.
            //albums = (await _httpClient.GetFromJsonAsync<List<Album>>("albums", options))!;

            return albums;
        }
    }
}
