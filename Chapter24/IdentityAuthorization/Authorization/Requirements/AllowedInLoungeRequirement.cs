using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthorizationApp.Authorization.Requirements
{
    // Интерфейс IAuthorizationRequirement идентифицирует класс как требование (requirement) авторизации.
    public class AllowedInLoungeRequirement : IAuthorizationRequirement { }
}
