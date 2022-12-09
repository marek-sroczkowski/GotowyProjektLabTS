using Microsoft.AspNetCore.Mvc.Filters;

namespace LabAuthorizationTS.Tests.Helpers
{
    public class FakeAdminFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = ClaimsPrincipalHelper.GetFakeAdminClaims();
            context.HttpContext.User = claimsPrincipal;

            await next();
        }
    }
}