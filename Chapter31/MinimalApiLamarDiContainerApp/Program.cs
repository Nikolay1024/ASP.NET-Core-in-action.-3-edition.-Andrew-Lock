using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Lamar.Scanning.Conventions;
using MinimalApiLamarDiContainerApp.Models;
using MinimalApiLamarDiContainerApp.Services;
using MinimalApiLamarDiContainerApp.Validators;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Настраиваем сервисы в UseLamar() вместо builder.Services.
builder.Host.UseLamar((ServiceRegistry services) =>
{
    // Вы добавляете сервисы платформы ASP.NET Core в ServiceRegistry.
    services.AddAuthorization();
    // Требуется, чтобы Lamar использовался для создания контроллеров веб-API.
    services.AddControllers().AddControllersAsServices();

    // Lamar может автоматически сканировать сборки в поисках сервисов, которые нужно зарегистрировать.
    services.Scan((IAssemblyScanner scanner) =>
    {
        // Указание сборки для сканирования.
        scanner.Assembly(Assembly.GetExecutingAssembly());
        // Регистрация сервисов, которые соответствуют соглашению
        // IPurchasingService (PurchasingService), ILeaderboard<T> (Leaderboard<T>).
        scanner.WithDefaultConventions();
        // Регистрация сервисов, которые не реализуют интерфейсы (ConcreteService).
        scanner.AddAllTypesOf<ConcreteService>();
        // Регистрация сервисов, которые реализуют IGamingService (CrosswordService, SudokuService).
        scanner.AddAllTypesOf<IGamingService>();
        // При запросе IValidator<T> использует необобщенную реализацию IValidator<T>.
        // Но если нет необобщенной реализации IValidator<T> использует DefaultValidator<T>.
        // Т.е. при запросе IValidator<UserModel> использует UserModelValidator.
        // Но при запросе IValidator<AvatarModel> использует DefaultValidator<AvatarModel>.
        scanner.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
    });

    // При запросе ILeaderboard<T> использует Leaderboard<T>.
    //services.AddTransient(typeof(ILeaderboard<>), typeof(Leaderboard<>));
    services.For(typeof(ILeaderboard<>)).Use(typeof(Leaderboard<>));
    // При запросе IUnitOfWork<T> запускает лямбда-выражение.
    // По умолчанию использует жизненный цикл Transient, но здесь метод Scoped() устанавливает Scoped.
    //services.AddScoped<IUnitOfWork>(_ => new UnitOfWork(3));
    services.For<IUnitOfWork>().Use(_ => new UnitOfWork(3)).Scoped();
    // При запросе IValidator<T> использует DefaultValidator<T>.
    // Т.е. при запросе IValidator<UserModel> использует DefaultValidator<UserModel>.
    // Также при запросе IValidator<AvatarModel> использует DefaultValidator<AvatarModel>.
    services.For(typeof(IValidator<>)).Add(typeof(DefaultValidator<>));
});

WebApplication app = builder.Build();

var container = (IContainer)app.Services;
Console.WriteLine(container.WhatDidIScan());
Console.WriteLine(container.WhatDoIHave(assembly: Assembly.GetExecutingAssembly()));

app.MapGet("/", (IPurchasingService purchasingService,
    ConcreteService concreteService,
    IEnumerable<IGamingService> gamingServices,
    ILeaderboard<UserModel> leaderboardUser,
    IUnitOfWork unitOfWork,
    IValidator<UserModel> validatorUser,
    IValidator<AvatarModel> validatorAvatar) => "Hello!");

app.Run();
