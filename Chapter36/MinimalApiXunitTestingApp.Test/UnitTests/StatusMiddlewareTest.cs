using Microsoft.AspNetCore.Http;
using MinimalApiXunitTestingApp.Middlewares;

namespace MinimalApiXunitTestingApp.Test.UnitTests
{
    // Модульное тестирование промежуточного ПО StatusMiddleware.
    public class StatusMiddlewareTest
    {
        [Fact]
        // Тестирование при указании совпадающего пути.
        public async Task ForMatchingRequest()
        {
            var httpContext1 = new DefaultHttpContext();
            httpContext1.Request.Path = "/ping";

            // DefaultHttpContext использует Stream.Null для объекта Response.Body, а это означает, что все, что
            // пишется в Body, теряется. Чтобы записать ответ и прочитать его для проверки содержимого, вы должны
            // заменить Body на MemoryStream.
            var memoryStream = new MemoryStream();
            httpContext1.Response.Body = memoryStream;

            RequestDelegate next = (HttpContext httpContext2) =>
            {
                httpContext2.Response.StatusCode = StatusCodes.Status404NotFound;
                return Task.CompletedTask;
            };

            var statusMiddleware = new StatusMiddleware(next);

            // Вызывает тестируемое промежуточное ПО, передавая контекст запроса.
            // Т.к. путь запроса /ping совпадает с путем промежуточного ПО /ping, то тестируемое промежуточное ПО
            // должно вернуть статусный код 200 с телом ответа pong.
            await statusMiddleware.Invoke(httpContext1);

            // Перематывает MemoryStream в начало и считывает тело ответа в строку.
            memoryStream.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(memoryStream);
            string content = await streamReader.ReadToEndAsync();

            // Проверяет статусный код.
            Assert.Equal(StatusCodes.Status200OK, httpContext1.Response.StatusCode);
            // Проверяет тело ответа.
            Assert.Equal("pong", content);
        }

        [Fact]
        // Тестирование при указании не совпадающего пути.
        public async Task ForNonMatchingRequest()
        {
            // Создает DefaultHttpContext и задает путь для запроса.
            var httpContext1 = new DefaultHttpContext();
            httpContext1.Request.Path = "/something-else";

            // Отслеживает, был ли выполнен следующий компонент промежуточного ПО.
            bool wasExecuted = false;

            // В этом примере должен быть вызван RequestDelegate, представляющий следующее промежуточного ПО в
            // конвейере обработки HTTP запроса.
            RequestDelegate next = (HttpContext httpContext2) =>
            {
                wasExecuted = true;
                return Task.CompletedTask;
            };

            // Создает экземпляр тестируемого промежуточного ПО, передавая следующее промежуточное ПО.
            var statusMiddleware = new StatusMiddleware(next);

            // Вызывает тестируемое промежуточное ПО, передавая контекст запроса.
            // Т.к. путь запроса /something-else не совпадает с путем промежуточного ПО /ping, то должно быть
            // вызвано следующее промежуточное ПО (RequestDelegate).
            await statusMiddleware.Invoke(httpContext1);

            // Проверяет, было ли вызвано следующее промежуточное ПО.
            Assert.True(wasExecuted);
        }
    }
}
