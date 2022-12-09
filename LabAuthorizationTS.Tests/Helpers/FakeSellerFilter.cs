using Microsoft.AspNetCore.Mvc.Filters;

namespace LabAuthorizationTS.Tests.Helpers
{
    public class FakeSellerFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = ClaimsPrincipalHelper.GetFakeSellerClaims();
            context.HttpContext.User = claimsPrincipal;

            await next();
        }
    }
}