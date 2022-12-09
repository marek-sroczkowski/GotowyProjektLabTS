using LabAuthorizationTS.Authorization.Requirements;
using LabAuthorizationTS.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace LabAuthorizationTS.Tests.Helpers
{
    public class FakePolicyEvaluatorForNotAdultCustomer : IPolicyEvaluator
    {
        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var claimsPrincipal = ClaimsPrincipalHelper.GetFakeNotAdultCustomerClaims();
            var ticket = new AuthenticationTicket(claimsPrincipal, "Test");
            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            var result = PolicyAuthorizationResult.Forbid();
            var role = context.User.GetUserRole();
            var rolesRequirement = policy.Requirements.FirstOrDefault(r => r is RolesAuthorizationRequirement);

            if (rolesRequirement != null && (rolesRequirement as RolesAuthorizationRequirement).AllowedRoles.Contains(role))
            {
                result = PolicyAuthorizationResult.Success();
            }

            var minimumAgeRequirement = policy.Requirements.FirstOrDefault(r => r is MinimumAgeRequirement);
            if (minimumAgeRequirement != null)
            {
                var dateOfBirth = context.User.GetUserBirthDate();
                if (dateOfBirth.HasValue && dateOfBirth.Value.IsAdult((minimumAgeRequirement as MinimumAgeRequirement).MinimumAge))
                {
                    result = PolicyAuthorizationResult.Success();
                }
            }

            return Task.FromResult(result);
        }
    }
}