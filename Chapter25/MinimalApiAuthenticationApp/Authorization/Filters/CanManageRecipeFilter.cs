using Microsoft.AspNetCore.Authorization;
using MinimalApiAuthenticationApp.Commands;
using MinimalApiAuthenticationApp.Data;
using MinimalApiAuthenticationApp.Services;

namespace MinimalApiAuthenticationApp.Authorization.Filters
{
    // Фильтр авторизации на основе ресурса для конечной точки API.
    public class CanManageRecipeFilter : IEndpointFilter
    {
        private readonly IAuthorizationService _authService;
        private readonly RecipeService _recipeService;

        // Внедрение сервиса авторизации из контейнера внедрения зависимостей.
        public CanManageRecipeFilter(IAuthorizationService authService, RecipeService recipeService)
        {
            _authService = authService;
            _recipeService = recipeService;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            // Получение id из параметра запроса.
            int id = ((RecipeUpdateCommand)context.Arguments[0]!).Id;
            // Получение рецепта по id, к которому пользователь пытается получить доступ.
            Recipe? recipe = await _recipeService.GetRecipe(id);

            // Авторизация на основе ресурса (рецепта).
            // Методу AuthorizeAsync() передается пользователь, ресурс и имя политики.
            AuthorizationResult result = await _authService.AuthorizeAsync(context.HttpContext.User, recipe,
                AuthorizationPolicyNames.CanManageRecipe);

            // Если авторизация не удалась, возвращает 403 Forbidden.
            if (!result.Succeeded)
                return Results.Forbid();

            // Если авторизация прошла успешно, конечная точка выполняется как обычно.
            // Если фильтры в конвейере фильтров конечной точки еще остались, то управление передается следующему
            // фильтру, иначе управление передается самой конечной точке.
            return await next(context);
        }
    }
}
