using Microsoft.AspNetCore.Http;
using MovieReviewerPlatform.Contracts.DTOs;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieDto>> GetAllAsync();
        Task<MovieDto?> GetByIdAsync(int id);
        Task<List<MovieDto>> GetByGenreIdAsync(int genreId);
        Task<List<MovieDto>> GetBySearchParametersAsync(int? releaseYear, string? title, int? genreId);
        Task<string> AddAsync(MovieDto newMovie, IFormFile? image);
        Task<MovieDto> UpdateAsync(int movieId, MovieDto newMovie);
        Task<double> GetAverageRatingBy(int movieId);
        Task<string> DeleteMovieImageByAsync(int movieId);
        Task<string> UpdateImageAsync(int movieId, IFormFile newImage);
        Task DeleteAsync(int id);
    }
}
