using Microsoft.AspNetCore.Mvc.Filters;

namespace LabAuthorizationTS.Tests.Helpers
{
    public class FakeNotAdultCustomerFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = ClaimsPrincipalHelper.GetFakeNotAdultCustomerClaims();
            context.HttpContext.User = claimsPrincipal;

            await next();
        }
    }
}