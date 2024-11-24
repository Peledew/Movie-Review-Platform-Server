using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IUserRatingRepository
    {
        Task<List<UserRating>> GetAllAsync();
        Task<List<UserRating>> GetByMovieIdAsync(int id);
        Task<UserRating?> GetByIdAsync(int userId, int movieId);

        Task AddAsync(UserRating rating);
        Task DeleteAsync(UserRating rating);
        Task SaveChangesAsync();
    }
}
