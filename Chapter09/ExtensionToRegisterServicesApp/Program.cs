using ExtensionToRegisterServicesApp.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Метод расширения AddEmailSender() регистрирует все сервисы, связанные с EmailSender.
builder.Services.AddEmailSender();
WebApplication app = builder.Build();

// Отправка электронной почты с использованием внедрения зависимостей.
app.MapGet("/example1/register/{username}", RegisterUser);

app.Run();


// Запрос у контейнера внедрения зависимостей (DI) экземпляра сервиса IEmailSender.
string RegisterUser(string username, IEmailSender emailSender)
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
class MessageFactory
{
    public static Email Create(string username) => new Email(username, "Спасибо за регистрацию");
}
