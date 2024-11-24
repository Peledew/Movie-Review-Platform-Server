using Microsoft.IdentityModel.Tokens;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieReviewerPlatform.Services
{
    public class AuthorizationService : Contracts.Interfaces.IAuthorizationService
    {
        private readonly IUserService _userService;
        public AuthorizationService(IUserService userService)
        {
            _userService = userService;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("your-very-secure-key-that-is-32-characters-or-longer!");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.Username}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);



        }

        public async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = await _userService.isRefreshTokenValidAsync(refreshToken);
            if (tokenInUser)
            {
                return await CreateRefreshToken();
            }
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("your-very-secure-key-that-is-32-characters-or-longer!");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }

        public async Task<TokenApiDto> ManageTokensAsync(User user)
        {
            user.Token = GenerateJwtToken(user);
            var newAccessToken = user.Token;
            var newRefreshToken = await CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _userService.SaveChangesAsync();

            return new TokenApiDto(newAccessToken, newRefreshToken);
        }

        public async Task<TokenApiDto> RefreshAsync(TokenApiDto tokenApiDto)
        {
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _userService.GetBy(username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return new TokenApiDto("invalid", "invalid");

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = await CreateRefreshToken();
            user.RefreshToken = newRefreshToken;

            await _userService.SaveChangesAsync();
            return new TokenApiDto(newAccessToken, newRefreshToken);
        }
    }
}
