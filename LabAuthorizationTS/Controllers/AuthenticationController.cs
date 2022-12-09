using LabAuthorizationTS.Models.Dtos.Authentications;
using LabAuthorizationTS.Models.Enums;
using LabAuthorizationTS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LabAuthorizationTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [SwaggerOperation(Summary = "Generates a jwt token when authentication is successfully")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResposneDto>> AuthenticateAsync([FromBody] AuthenticationRequestDto authRequest)
        {
            var authResult = await authenticationService.AuthenticateAsync(authRequest);

            return authResult.Status == AuthenticationStatus.InvalidUsernameOrPassword
                ? BadRequest(authResult)
                : Ok(authResult);
        }
    }
}