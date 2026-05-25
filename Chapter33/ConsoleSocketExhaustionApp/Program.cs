// Сокеты - это пара ip и порт (172.64.107.5:80).
// Чтобы увидеть используемые сокеты, запустите команду netstat в консоли.
// В Windows используйте команду "netstat -n", чтобы пропустить разрешение DNS-имен.
int i = 1;
while (true)
{
    // Неправильный способ использования класса HttpClient.
    // Данный пример имеет ошибку, вызванную исчерпанием сокетов. Каждый раз при создании подключения по протоколу
    // HTTP приложение задействует новый порт в диапазоне 0-65535. И каждый раз при освобождении объекта HttpClient
    // соединение переходит в состояние TIME_WAIT, в котором порт не может быть использован еще 240 секунд (для
    // Windows).
    using var httpClient = new HttpClient();
    // Настраивает базовый URL-адрес, используемый для выполнения запросов с помощью HttpClient.
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
    // Выполняет GET-запрос к тестовому API JsonPlaceholder.
    HttpResponseMessage hrm = await httpClient.GetAsync("albums/1");
    Console.WriteLine($"Response {i}: {hrm.StatusCode}");
    i++;
}
