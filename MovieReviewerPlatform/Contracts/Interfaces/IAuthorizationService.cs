using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;
using System.Security.Claims;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IAuthorizationService
    {
        string GenerateJwtToken(User user);
        Task<string> CreateRefreshToken();
        ClaimsPrincipal GetPrincipleFromExpiredToken(string token);
        Task<TokenApiDto> ManageTokensAsync(User user);
        Task<TokenApiDto> RefreshAsync(TokenApiDto tokenApiDto);
    }
}
