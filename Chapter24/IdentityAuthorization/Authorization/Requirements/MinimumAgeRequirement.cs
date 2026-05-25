using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthorizationApp.Authorization.Requirements
{
    // Интерфейс IAuthorizationRequirement идентифицирует класс как требование (requirement) авторизации.
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        // Обработчики (handlers) могут использовать установленный минимальный возраст, чтобы определить,
        // выполнено ли требование (requirement).
        public int MinimumAge { get; }

        // При создании требования (requirement) указывается минимальный возраст.
        public MinimumAgeRequirement(int minimumAge) => MinimumAge = minimumAge;
    }
}
