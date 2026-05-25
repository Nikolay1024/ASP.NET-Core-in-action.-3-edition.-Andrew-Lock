WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Регистрация сервиса в контейнере внедрения зависимостей (DI), чтобы DI знал, какую реализацию использовать
// для каждого запрашиваемого сервиса.
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<MessageFactory>();
builder.Services.AddSingleton<NetworkClient>();
builder.Services.AddSingleton((IServiceProvider provider) => new EmailServerSettings("smtp.server.com", 25));
//builder.Services.AddSingleton(new EmailServerSettings("smtp.server.com", 25));
WebApplication app = builder.Build();

// Отправка электронной почты без внедрения зависимостей при создании зависимостей вручную.
app.MapGet("/example1/register1/{username}", RegisterUser1);
// Отправка электронной почты с использованием внедрения зависимостей.
app.MapGet("/example1/register2/{username}", RegisterUser2);

// Для получения сервиса IEmailSender можно запросить его напрямую из контейнера внедрения зависимостей (DI) с
// помощью метода GetRequiredService<T>(). Этот подход называется паттерн "Локатор сервисов".
IEmailSender emailSender = app.Services.GetRequiredService<IEmailSender>();
LinkGenerator links = app.Services.GetRequiredService<LinkGenerator>();

app.Run();


string RegisterUser1(string username)
{
    var emailSender = new EmailSender(new MessageFactory(),
        new NetworkClient(new EmailServerSettings("smtp.server.com", 25)));
    emailSender.SendEmail(username);
    return $"Письмо отправлено {username}.";
}
// Запрос у контейнера внедрения зависимостей (DI) экземпляра сервиса IEmailSender. DI заботится о создании
// графа зависимостей, т.е. графа всех зависимых объектов, необходимых для создания корневого объекта
// IEmailSender. В данном контексте под сервисом подразумевается любой класс или интерфейс, объект которого DI
// либо создает, либо передает ранее созданный объект.
// Основным преимуществом использования DI является достижение слабосвязанного кода, путем программирования на
// уровне интерфейса.
// Встроенный в ASP.NET Core DI поддерживает внедрение серсисов в качестве параметра в конструктор и методы.
string RegisterUser2(string username, IEmailSender emailSender)
{
    emailSender.SendEmail(username);
    return $"Письмо отправлено {username}.";
}

record Email(string Address, string Message);
record EmailServerSettings(string Host, int Port);
interface IEmailSender
{
    void SendEmail(string username);
}
class EmailSender : IEmailSender
{
    readonly MessageFactory MessageFactory;
    readonly NetworkClient NetworkClient;

    public EmailSender(MessageFactory messageFactory, NetworkClient networkClient)
    {
        MessageFactory = messageFactory;
        NetworkClient = networkClient;
    }

    public void SendEmail(string username)
    {
        Email email = MessageFactory.Create(username);
        NetworkClient.SendEmail(email);
        Console.WriteLine($"Письмо отправлено {username}.");
    }
}
class MessageFactory
{
    public static Email Create(string username) => new Email(username, "Спасибо за регистрацию");
}
class NetworkClient
{
    readonly EmailServerSettings EmailServerSettings;

    public NetworkClient(EmailServerSettings emailServerSettings) => EmailServerSettings = emailServerSettings;

    public void SendEmail(Email email)
    {
        Console.WriteLine($"Подключение к серверу {EmailServerSettings.Host}:{EmailServerSettings.Port}.");
        Console.WriteLine($"Письмо отправлено {email.Address}: {email.Message}.");
    }
}
