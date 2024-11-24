using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync();
        Task<List<Movie>> GetByGenreIdAsync(int genreId);
        Task<List<Movie>> GetBySearchParametersAsync(int? releaseYear, string? title, int? genreId);
        Task<Movie?> GetByIdAsync(int id);

        Task AddAsync(Movie movie);
        Task DeleteAsync(Movie movie);
        Task SaveChangesAsync();
    }
}
