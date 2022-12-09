using LabAuthorizationTS.Authorization.Requirements;
using LabAuthorizationTS.Authorization.Utils;
using LabAuthorizationTS.Extensions;
using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace LabAuthorizationTS.Authorization.Handlers
{
    public class ProductResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, ProductDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, ProductDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            //TODO Admin może wszystko, jeśli produkt nie ma ograniczenia wiekowego, a użytkownik to Customer to Succeed
            //jeśli produkt ma ograniczenie wiekowe, a użytkownik to pełnoletni Customer to Succeed
            //jeśli użytkownik to Seller, i jest to jego produkt to Succeed

            var role = context.User.GetUserRole();
            if (role == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }

            if (requirement.OperationType == OperationType.Read && !resource.OnlyForAdults)
            {
                context.Succeed(requirement);
            }

            var dateOfBirth = context.User.GetUserBirthDate();
            if (requirement.OperationType == OperationType.Read && resource.OnlyForAdults
                && dateOfBirth.HasValue && dateOfBirth.Value.IsAdult(18))
            {
                context.Succeed(requirement);
            }

            var userId = context.User.GetUserId();
            if (role == UserRole.Seller.ToString() && resource.UserId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}