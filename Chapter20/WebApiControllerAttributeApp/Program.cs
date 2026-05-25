using WebApiControllerAttributeApp.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Метод ConfigureApiBehaviorOptions() позволяет управлять применяемыми соглашениями атрибута [ApiController].
// SuppressModelStateInvalidFilter подавляет в методах действий неявную проверку состояние модели, и в случае
// невалидности модели также подавляет отправку кода состояния 400 Bad Request в формате Problem Details.
builder.Services.AddControllers()
    /*.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)*/;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<FruitService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
