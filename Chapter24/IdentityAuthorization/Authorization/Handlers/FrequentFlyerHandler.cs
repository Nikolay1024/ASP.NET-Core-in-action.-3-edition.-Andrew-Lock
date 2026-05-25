using IdentityAuthorizationApp.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthorizationApp.Authorization.Handlers
{
    // Обработчик реализует AuthorizationHandler<TRequirement>.
    public class FrequentFlyerHandler : AuthorizationHandler<AllowedInLoungeRequirement>
    {
        // Вы должны переопределить абстрактный метод HandleRequirementAsync().
        protected override Task HandleRequirementAsync(
            // Контекст содержит такие сведения, как пользовательский объект ClaimsPrincipal.
            AuthorizationHandlerContext context,
            // Экземпляр требования для обработки.
            AllowedInLoungeRequirement requirement)
        {
            // Проверяет, есть ли у пользователя утверждение (cliam) FrequentFlyerClass со значением Gold.
            bool hasClaim = context.User.HasClaim(Claims.FrequentFlyerClass, "Gold");
            if (hasClaim)
                // Если у пользователя было необходимое утверждение (cliam), отмечаем требование (requirement)
                // как удовлетворенное, вызывая метод Succeed().
                context.Succeed(requirement);

            // Если у пользователя не было необходимого утверждения (cliam), ничего не предпринимаем.
            return Task.CompletedTask;
        }

        // Достаточно одного успешно выполненного обработчика (handler), ассоциированного с требованием (requirement),
        // чтобы требование считалось удовлетворенным. Однако, если среди обработчиков есть хотя бы один неудачно
        // выполненный (с вызовом метода Fail()), то все требование считается неудовлетворенным.
    }
}
