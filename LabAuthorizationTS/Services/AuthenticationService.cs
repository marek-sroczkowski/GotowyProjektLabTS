using AutoMapper;
using LabAuthorizationTS.Identity.Interfaces;
using LabAuthorizationTS.Models.Dtos.Authentications;
using LabAuthorizationTS.Models.Dtos.Users;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Models.Enums;
using LabAuthorizationTS.Repositories.Interfaces;
using LabAuthorizationTS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LabAuthorizationTS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> userRepository;
        private readonly IJwtProvider jwtProvider;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IMapper mapper;

        public AuthenticationService(IRepository<User> userRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.jwtProvider = jwtProvider;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
        }

        public async Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationRequestDto authentication)
        {
            var user = await userRepository.FindFirstAsync(u => u.Username == authentication.Username);
            if (user == null)
            {
                return new AuthenticationResposneDto { Status = AuthenticationStatus.InvalidUsernameOrPassword };
            }

            var authResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, authentication.Password);
            if (authResult != PasswordVerificationResult.Success)
            {
                return new AuthenticationResposneDto { Status = AuthenticationStatus.InvalidUsernameOrPassword };
            }

            var userDto = mapper.Map<UserDto>(user);
            var token = jwtProvider.GenerateJwtToken(userDto);

            return new AuthenticationResposneDto { JwtToken = token, User = userDto };
        }
    }
}