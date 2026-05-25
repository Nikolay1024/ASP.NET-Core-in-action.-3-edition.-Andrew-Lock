namespace MinimalApiConfigureOptionsApp.OptionsProviders
{
    public class GuidService
    {
        private readonly Guid guid = Guid.NewGuid();

        // Возвращать фиксированный GUID на протяжении жизненного цикла сервиса.
        public Guid GetGuid() => guid;
    }
}
