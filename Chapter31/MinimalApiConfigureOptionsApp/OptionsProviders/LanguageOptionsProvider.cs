namespace MinimalApiConfigureOptionsApp.OptionsProviders
{
    public class LanguageOptionsProvider : ILanguageOptionsProvider
    {
        public string[] GetSupportedLanguages()
        {
            // Здесь может быть загрузка настроек из базы данных/удаленного API.
            return new string[] { "English", "French", "Spanish", "German" };
        }
    }
}
