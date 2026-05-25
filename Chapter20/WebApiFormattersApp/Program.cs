using Microsoft.AspNetCore.Mvc.Formatters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// По умолчанию Web API возвращает только MIME-типы text/json, text/plain и text/html.
builder.Services
    .AddControllers(options =>
    {
        // Принимать и обрабатывать заголовки Accept из браузера.
        options.RespectBrowserAcceptHeader = true;

        // Возвращать код 406, если запрашиваемый формат данных ответа не поддерживается сервером.
        //options.ReturnHttpNotAcceptable = true;

        // Удаляет форматер вывода, который форматирует строки как text/plain.
        options.OutputFormatters.RemoveType<StringOutputFormatter>();
    })
    // Добавление форматеров ввода и вывода для XML.
    .AddXmlSerializerFormatters()
    // Настраивает JSON форматирование PascalCase вместо форматирования camelCase по умолчанию.
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
