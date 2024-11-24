using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IUserCommentRepository
    {
        Task<List<UserComment>> GetAllAsync();

        Task<UserComment?> GetByIdAsync(int id);

        Task AddAsync(UserComment comment);
        Task DeleteAsync(UserComment comment);
        Task SaveChangesAsync();
    }
}
