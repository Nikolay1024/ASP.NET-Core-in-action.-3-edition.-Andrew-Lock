using IdentityAuthorizationApp.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityAuthorizationApp.Authorization.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinimumAgeRequirement requirement)
        {
            string? dateOfBirthStr = context.User.FindFirstValue(ClaimTypes.DateOfBirth);

            if (dateOfBirthStr == null)
                return Task.CompletedTask;

            DateTime dateOfBirth = Convert.ToDateTime(dateOfBirthStr);
            bool over18Years = dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today;

            if (over18Years)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
