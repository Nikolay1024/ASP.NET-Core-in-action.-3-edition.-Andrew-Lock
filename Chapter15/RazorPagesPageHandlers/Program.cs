using RazorPagesPageHandlers.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<SearchService>();

WebApplication app = builder.Build();

// StatusCodePagesMiddleware позволяет предоставлять удобные сообщения об ошибках, когда конвейер возвращает
// необработанный код состояния ответа на ошибку. Это важно для обеспечения единообразного взаимодействия с
// пользователем при возврате кода состояния ошибок, например ошибок 404, когда URL-адрес не соответствует
// конечной точке.

// Повторно запускает запрос в конвейер при перехвате кодов состояния 4xx, 5xx без тела запроса.
// Позволяет отобразить ошибку сохранив код состояния 404 в рамках одного HTTP-запроса.
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Возвращает ответ с кодом состояния 302, указывая браузеру отправить второй запрос, на этот раз для URL-адреса ошибки.
// Отображает ошибку с кодом состояния 200 (что не соответствет тексту ошибки). Требует для работы два запроса.
// Данный метод полезен, когда другое приложение генерирует страницу с ошибкой.
//app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
