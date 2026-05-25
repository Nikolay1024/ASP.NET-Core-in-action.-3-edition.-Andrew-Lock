using Microsoft.AspNetCore.Authorization;
using MinimalApiAuthenticationApp.Authorization.Requirements;
using MinimalApiAuthenticationApp.Data;

namespace MinimalApiAuthenticationApp.Authorization.Handlers
{
    public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, Recipe?>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsRecipeOwnerRequirement requirement, Recipe? resource)
        {
            if (resource is null || resource.CreatedBy == context.User.Identity?.Name)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
