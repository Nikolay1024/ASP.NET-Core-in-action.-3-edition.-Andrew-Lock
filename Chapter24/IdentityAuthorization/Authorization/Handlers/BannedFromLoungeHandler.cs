using IdentityAuthorizationApp.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthorizationApp.Authorization.Handlers
{
    // Обработчик реализует AuthorizationHandler<TRequirement>.
    public class BannedFromLoungeHandler : AuthorizationHandler<AllowedInLoungeRequirement>
    {
        // Вы должны переопределить абстрактный метод HandleRequirementAsync().
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AllowedInLoungeRequirement requirement)
        {
            // Проверяет, есть ли у пользователя утверждение (claim) IsBannedFromLounge.
            bool isBanned = context.User.HasClaim(c => c.Type == Claims.IsBannedFromLounge);
            if (isBanned)
                // Если у пользователя есть утверждение, отмечаем требование как неудовлетворенное,
                // вызывая метод Fail(). Вся политика не будет удовлетворена.
                context.Fail();

            // Если у пользователя не было необходимого утверждения (cliam), ничего не предпринимаем.
            return Task.CompletedTask;
        }
    }
}
