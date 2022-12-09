using LabAuthorizationTS.Models.Dtos.Tokens;
using LabAuthorizationTS.Models.Dtos.Users;

namespace LabAuthorizationTS.Identity.Interfaces
{
    public interface IJwtProvider
    {
        TokenDto GenerateJwtToken(UserDto user);
    }
}