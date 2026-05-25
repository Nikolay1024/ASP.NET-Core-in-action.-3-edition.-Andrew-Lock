using IdentityAuthorization2App.Authorization.Requirements;
using IdentityAuthorization2App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthorization2App.Authorization.Handlers
{
    // Реализует базовый класс с указанием требования (requirement) и типа ресурса (resource).
    public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, Recipe>
    {
        private readonly UserManager<AppUser> _userManager;

        // Сервис UserManager<AppUser> внедряется в конструктор из контейнера внедрения зависимостей.
        public IsRecipeOwnerHandler(UserManager<AppUser> userManager) => _userManager = userManager;

        // Помимо контекста и требования, вам также предоставляется экземпляр ресурса.
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsRecipeOwnerRequirement requirement, Recipe resource)
        {
            AppUser? appUser = await _userManager.GetUserAsync(context.User);
            // Если вы не прошли аутентификацию, appUser будет иметь значение null.
            if (appUser == null)
                return;

            // Проверяет, создал ли текущий пользователь рецепт, проверяя свойство CreatedBy.
            if (resource.CreatedBy == appUser.Id)
                // Если пользователь создал рецепт, использует метод Succeed.
                // В противном случае ничего не делает.
                context.Succeed(requirement);
        }
    }
}
