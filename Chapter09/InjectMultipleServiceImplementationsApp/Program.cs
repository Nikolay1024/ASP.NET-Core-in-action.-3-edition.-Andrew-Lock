using Microsoft.Extensions.DependencyInjection.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Регистрация трех реализаций серивса IMessageSender.
builder.Services.AddSingleton<IMessageSender, EmailSender>();
builder.Services.AddSingleton<IMessageSender, SmsSender>();
builder.Services.AddSingleton<IMessageSender, FacebookSender>();

// Условное добавление сервиса с помощью метода TryAddSingleton(). Регистрация не будет произведена, т.к.
// сервис IMessageSender уже имеет зарегистрированные реализации.
builder.Services.TryAddSingleton<IMessageSender, UnregisteredSender>();

// Замена первой зарегистрированной реализации сервиса IMessageSender (EmailSender) на реализацию SmsSender.
// При использовании метода Replace() вы должны указать тот же жизненный цикл (ServiceLifetime.Singleton).
//builder.Services.Replace(new ServiceDescriptor(typeof(IMessageSender), typeof(SmsSender),
//    ServiceLifetime.Singleton));

WebApplication app = builder.Build();

// Отправка сообщений с использованием трех реализаций сервиса IMessageSender.
app.MapGet("/example1/register1/{username}", RegisterUser1);
// Отправка сообщения с использованием последней зарегистрированной реализации FacebookSender.
app.MapGet("/example1/register2/{username}", RegisterUser2);

app.Run();


// Запрос у контейнера внедрения зависимостей (DI) всех зарегистрированных реализаций сервиса IMessageSender.
string RegisterUser1(string username, IEnumerable<IMessageSender> senders)
{
    foreach (IMessageSender sender in senders)
        sender.SendMessage($"Привет {username}.");
    return $"Сообщения отправлены {username}.";
}
string RegisterUser2(string username, IMessageSender sender)
{
    sender.SendMessage($"Привет {username}.");
    return $"Сообщение отправлено {username}.";
}

interface IMessageSender
{
    void SendMessage(string message);
}
class EmailSender : IMessageSender
{
    public void SendMessage(string message) => Console.WriteLine($"Отправлена почта: {message}");
}
class SmsSender : IMessageSender
{
    public void SendMessage(string message) => Console.WriteLine($"Отпралено SMS: {message}");
}
class FacebookSender : IMessageSender
{
    public void SendMessage(string message) => Console.WriteLine($"Отправлено в Facebook: {message}");
}
class UnregisteredSender : IMessageSender
{
    public void SendMessage(string message)
    {
        throw new Exception("Реализация сервиса не регистрировалась, поэтому не будет вызываться.");
    }
}
