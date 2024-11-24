using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task RegisterAsync(User user);
        Task <List<User>> GetAllAsync();
        Task<bool> IsLoggedInWithRefreshToken(string refreshToken);
        Task SaveChangesAsync();

    }
}
