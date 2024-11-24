using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(LogInDto user);
        Task<string> RegisterAsync(UserDto userData);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<bool> isRefreshTokenValidAsync(string RefreshToken);
        Task SaveChangesAsync();
        Task<User> GetBy(string username);
        bool ValidatePasswordAsync(string password, User user);
        int GetCurrentUserId();

    }

}
