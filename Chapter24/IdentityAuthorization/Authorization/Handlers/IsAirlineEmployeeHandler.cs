using IdentityAuthorizationApp.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthorizationApp.Authorization.Handlers
{
    // Обработчик реализует AuthorizationHandler<TRequirement>.
    public class IsAirlineEmployeeHandler : AuthorizationHandler<AllowedInLoungeRequirement>
    {
        // Вы должны переопределить абстрактный метод HandleRequirementAsync().
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AllowedInLoungeRequirement requirement)
        {
            // Проверяет, есть ли у пользователя утверждение EmployeeNumber.
            bool hasClaim = context.User.HasClaim(c => c.Type == Claims.EmployeeNumber);
            if (hasClaim)
                // Если у пользователя было необходимое утверждение (cliam), отмечаем требование (requirement)
                // как удовлетворенное, вызывая метод Succeed().
                context.Succeed(requirement);

            // Если у пользователя не было необходимого утверждения (cliam), ничего не предпринимаем.
            return Task.CompletedTask;
        }
    }
}
