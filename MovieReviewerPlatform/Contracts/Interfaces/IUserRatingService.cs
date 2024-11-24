using MovieReviewerPlatform.Contracts.DTOs;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IUserRatingService
    {
        Task<List<UserRatingDto>> GetAllAsync();
        Task<List<UserRatingDto>> GetByMovieIdAsync(int id);
        Task<UserRatingDto?> GetByIdAsync(int userId, int movieId);
        Task<string> AddAsync(UserRatingDto newRating);
        Task<bool> TryRatingAsync(int movieId);
    }
}
