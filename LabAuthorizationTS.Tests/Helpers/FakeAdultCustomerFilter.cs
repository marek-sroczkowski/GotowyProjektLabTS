using Microsoft.AspNetCore.Mvc.Filters;

namespace LabAuthorizationTS.Tests.Helpers
{
    public class FakeAdultCustomerFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = ClaimsPrincipalHelper.GetFakeAdultCustomerClaims();
            context.HttpContext.User = claimsPrincipal;

            await next();
        }
    }
}