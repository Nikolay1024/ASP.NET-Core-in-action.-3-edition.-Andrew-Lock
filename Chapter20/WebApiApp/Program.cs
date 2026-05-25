WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Метод AddControllers() добавляет в приложение сервисы для контроллеров API,
// но не добавляет сервисы для отрисовки представлений Razor.
builder.Services.AddControllers();
// Сервисы, необходимые для создания спецификации OpenAPI.
builder.Services.AddEndpointsApiExplorer();
// Сервисы, необходимые для работы инструмента Swagger.
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Добавляет промежуточное ПО Swagger UI для изучения веб-API.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Регистрирует методы действий в контроллерах API в качестве конечных точек, используя явную маршрутизацию.
app.MapControllers();

app.Run();
