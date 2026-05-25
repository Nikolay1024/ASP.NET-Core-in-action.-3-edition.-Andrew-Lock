namespace ExtensionToRegisterServicesApp.Extensions
{
    public static class EmailSenderServiceCollectionExtensions
    {
        // Создаем метод расширения для типа IServiceCollection с помощью ключевого слова this.
        public static IServiceCollection AddEmailSender(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<MessageFactory>();
            services.AddSingleton<NetworkClient>();
            services.AddSingleton(provider => new EmailServerSettings("smtp.server.com", 25));

            // Согласно конвенции "Цепочка методов", возвращаем IServiceCollection.
            return services;
        }
    }
}
