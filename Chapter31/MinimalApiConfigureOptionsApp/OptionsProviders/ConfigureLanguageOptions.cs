using Microsoft.Extensions.Options;
using MinimalApiConfigureOptionsApp.Options;

namespace MinimalApiConfigureOptionsApp.OptionsProviders
{
    public class ConfigureLanguageOptions : IConfigureOptions<LanguageOptions>
    {
        private readonly ILanguageOptionsProvider _optionsProvider;

        // В сервис IConfigureOptions<T> возможно внедрять сервисы только с жизненным циклом Singleton.
        public ConfigureLanguageOptions(ILanguageOptionsProvider optionsProvider) =>
            _optionsProvider = optionsProvider;

        // Метод Configure() вызывается, когда требуется экземпляр IOptions<LanguageOptions>.
        public void Configure(LanguageOptions options) =>
            // Использует внедренный сервис для загрузки значений.
            options.SupportedLanguages = _optionsProvider.GetSupportedLanguages();
    }
}
