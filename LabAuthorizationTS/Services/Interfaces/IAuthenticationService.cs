using LabAuthorizationTS.Models.Dtos.Authentications;

namespace LabAuthorizationTS.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationRequestDto authentication);
    }
}