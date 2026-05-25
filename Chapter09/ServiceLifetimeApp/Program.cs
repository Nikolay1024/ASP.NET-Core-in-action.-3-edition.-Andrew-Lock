WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseDefaultServiceProvider(options =>
{
    // Свойство проверяет жизненный цикл сервисов. Чтобы сервис использовал только те зависимости, жизненный
    // цикл которых превышает или эквивалентен жизненному циклу сервиса.
    options.ValidateScopes = false;
    // Свойство проверяет, что для каждого зарегистрированного сервиса зарегистрированы все его зависимости.
    options.ValidateOnBuild = false;

    // Значения по умолчанию.
    // Проверки выполняются только в среде разработки, т.к. они плохо влияют на производительность.
    //options.ValidateScopes = builder.Environment.IsDevelopment();
    //options.ValidateOnBuild = builder.Environment.IsDevelopment();
});

// Возвращает уникальный объект при каждом внедрении сервиса.
builder.Services.AddTransient<TransientRepository>();
builder.Services.AddTransient<TransientDataContext>();

// Возвращает одинаковый объект на протяжении одного HTTP запроса.
builder.Services.AddScoped<ScopedRepository>();
builder.Services.AddScoped<ScopedDataContext>();

// Возвращает одинаковый объект на протяжении жизненного цикла приложения.
builder.Services.AddSingleton<SingletonRepository>();
builder.Services.AddSingleton<SingletonDataContext>();

// Сервис должен использовать только те зависимости, жизненный цикл которых превышает или эквивалентен
// жизненному циклу сервиса.
// Singleton сервис может безопасно использовать только Singleton зависимости.
// Scoped сервис может безопасно использовать Scoped или Singleton зависимости.
// Transient сервис может безопасно использовать Transient, или Scoped, или Singleton зависимости.
//
// В примере сервис CapturingRepository зависит от сервиса CapturedDataContext.
// CapturingRepository имеет более длительный жизненный цикл (Singleton), чем CapturedDataContext (Scope).
// В результате в среде разработки (Environment.Development) возникнет исключение. А в другой среде при втором
// запросе сервис CapturingRepository захватит зависимость CapturedDataContext. Это приведет к тому, что
// CapturedDataContext будет вести себя так, как будто он был зарегистрирован как Singleton, хотя на самом
// деле он Scoped.
// В данном примере сервис CapturingRepository является захватывающий, а CapturedDataContext - захваченный.
builder.Services.AddSingleton<CapturingRepository>();
builder.Services.AddScoped<CapturedDataContext>();

WebApplication app = builder.Build();

app.MapGet("/", () => @"Перейдите на /transient, /scoped, /singleton, /captured.

Обновите страницу несколько раз, чтобы увидеть взаимосвязь между значениями
Repository и DataContext, а также то, как они меняются при обновлении страницы.");

List<string> transients = new();
List<string> scopeds = new();
List<string> singletons = new();
List<string> captures = new();

app.MapGet("/transient", Transient);
app.MapGet("/scoped", Scoped);
app.MapGet("/singleton", Singleton);
app.MapGet("/captured", Captured);

string Transient(TransientRepository repo, TransientDataContext db) => RowCounts(repo, db, transients);
string Scoped(ScopedRepository repo, ScopedDataContext db) => RowCounts(repo, db, scopeds);
string Singleton(SingletonRepository repo, SingletonDataContext db) => RowCounts(repo, db, singletons);
string Captured(CapturingRepository repo, CapturedDataContext db) => RowCounts(repo, db, captures);

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
class CapturedDataContext : DataContext { }

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
class CapturingRepository : Repository
{
    public CapturingRepository(CapturedDataContext db) : base(db) { }
}
