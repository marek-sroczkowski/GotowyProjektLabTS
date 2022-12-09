using LabAuthorizationTS.Config;
using LabAuthorizationTS.Identity.Interfaces;
using LabAuthorizationTS.Models.Dtos.Tokens;
using LabAuthorizationTS.Models.Dtos.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabAuthorizationTS.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly AppSettings settings;

        public JwtProvider(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        public TokenDto GenerateJwtToken(UserDto user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("DateOfBirth", user.BirthDate.ToShortDateString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(settings.JwtExpireDays);

            var token = new JwtSecurityToken
            (
                settings.JwtIssuer,
                settings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return new TokenDto
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationDate = expires
            };
        }
    }
}