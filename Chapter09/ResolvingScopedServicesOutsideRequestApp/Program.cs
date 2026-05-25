WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Возвращает уникальный объект при каждом внедрении сервиса.
builder.Services.AddTransient<TransientRepository>();
builder.Services.AddTransient<TransientDataContext>();

// Возвращает одинаковый объект на протяжении одного HTTP запроса.
builder.Services.AddScoped<ScopedRepository>();
builder.Services.AddScoped<ScopedDataContext>();

// Возвращает одинаковый объект на протяжении жизненного цикла приложения.
builder.Services.AddSingleton<SingletonRepository>();
builder.Services.AddSingleton<SingletonDataContext>();

WebApplication app = builder.Build();

app.MapGet("/", () => @"Перейдите на /transient, /scoped, /singleton.

Обновите страницу несколько раз, чтобы увидеть взаимосвязь между значениями
Repository и DataContext, а также то, как они меняются при обновлении страницы.");

List<string> transients = new();
List<string> scopeds = new();
List<string> singletons = new();

app.MapGet("/transient", Transient);
app.MapGet("/scoped", Scoped);
app.MapGet("/singleton", Singleton);

// T service = app.Services.GetRequiredService<T>();
// Сервисы, разрешенные из корневого контейнера внедрения зависимостей (DI), удаляются только после завершения
// работы приложения. Такое поведение подходит только сервисам с жизненным циклом Singleton.
// T service = scope.ServiceProvider.GetRequiredService<T>();
// Сервисы, разрешенные из IServiceScope, удаляются при удалении IServiceScope.
async Task<string> Transient()
{
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
    TransientRepository repo = scope.ServiceProvider.GetRequiredService<TransientRepository>();
    TransientDataContext db = scope.ServiceProvider.GetRequiredService<TransientDataContext>();
    return RowCounts(repo, db, transients);
}
async Task<string> Scoped()
{
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
    ScopedRepository repo = scope.ServiceProvider.GetRequiredService<ScopedRepository>();
    ScopedDataContext db = scope.ServiceProvider.GetRequiredService<ScopedDataContext>();
    return RowCounts(repo, db, scopeds);
}
string Singleton()
{
    SingletonRepository repo = app.Services.GetRequiredService<SingletonRepository>();
    SingletonDataContext db = app.Services.GetRequiredService<SingletonDataContext>();
    return RowCounts(repo, db, singletons);
}

static string RowCounts(Repository repo, DataContext db, List<string> previous)
{
    string counts = $"{repo.GetType().Name}: {repo.RowCount:000}, {db.GetType().Name}: {db.RowCount:000}";

    string result = $@"Текущие значения:
{counts}

Предыдущие значения:
{string.Join(Environment.NewLine, previous)}";

    previous.Insert(0, counts);
    return result;
}

app.Run();


class DataContext
{
    public int RowCount { get; } = Random.Shared.Next(1, 1000);
}
class TransientDataContext : DataContext { }
class ScopedDataContext : DataContext { }
class SingletonDataContext : DataContext { }

class Repository
{
    readonly DataContext DataContext;
    public int RowCount => DataContext.RowCount;
    public Repository(DataContext db) => DataContext = db;
}
class TransientRepository : Repository
{
    public TransientRepository(TransientDataContext db) : base(db) { }
}
class ScopedRepository : Repository
{
    public ScopedRepository(ScopedDataContext db) : base(db) { }
}
class SingletonRepository : Repository
{
    public SingletonRepository(SingletonDataContext db) : base(db) { }
}
