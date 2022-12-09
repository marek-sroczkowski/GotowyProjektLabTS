using LabAuthorizationTS.Models.Enums;
using System.Security.Claims;

namespace LabAuthorizationTS.Tests.Helpers
{
    public class ClaimsPrincipalHelper
    {
        public static ClaimsPrincipal GetFakeAdminClaims()
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, UserRole.Admin.ToString()),
                    new Claim("DateOfBirth", DateTime.Now.AddYears(-40).ToShortDateString()),
                }));

            return claimsPrincipal;
        }

        public static ClaimsPrincipal GetFakeAdultCustomerClaims()
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Role, UserRole.Customer.ToString()),
                    new Claim("DateOfBirth", DateTime.Now.AddYears(-30).ToShortDateString()),
                }));

            return claimsPrincipal;
        }

        public static ClaimsPrincipal GetFakeNotAdultCustomerClaims()
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "3"),
                    new Claim(ClaimTypes.Role, UserRole.Customer.ToString()),
                    new Claim("DateOfBirth", DateTime.Now.AddYears(-15).ToShortDateString()),
                }));

            return claimsPrincipal;
        }

        public static ClaimsPrincipal GetFakeSellerClaims()
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "4"),
                    new Claim(ClaimTypes.Role, UserRole.Seller.ToString()),
                    new Claim("DateOfBirth", DateTime.Now.AddYears(-35).ToShortDateString()),
                }));

            return claimsPrincipal;
        }
    }
}