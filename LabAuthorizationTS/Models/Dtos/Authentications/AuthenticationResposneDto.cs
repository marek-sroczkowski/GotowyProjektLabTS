using LabAuthorizationTS.Models.Dtos.Tokens;
using LabAuthorizationTS.Models.Dtos.Users;
using LabAuthorizationTS.Models.Enums;

namespace LabAuthorizationTS.Models.Dtos.Authentications
{
    public class AuthenticationResposneDto
    {
        public AuthenticationStatus Status { get; set; } = AuthenticationStatus.Success;
        public UserDto User { get; set; }
        public TokenDto JwtToken { get; set; }
    }
}