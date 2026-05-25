WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    // DeveloperExceptionPageMiddleware предоставляет информацию об ошибках при разработке приложения.
    // Компонент не следует использовать в промышленном окружении.
    app.UseDeveloperExceptionPage();
else
    // ExceptionHandlerMiddleware предоставляет информацию об ошибках пользователю в выбранном виде.
    // Компонент безопасен для использования в промышленном окружении, поскольку не раскрывает
    // конфиденциальные сведения о приложении.
    app.UseExceptionHandler("/error");

app.MapGet("/", () => "Hello World!");
//  При добавлении компонента ExceptionHandlerMiddleware указывается путь к странице ошибки,
//  которую увидит пользователь.
app.MapGet("/error", () => "Возникла внутренняя ошибка сервера.");

app.Run();
