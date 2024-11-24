using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IGenreService
    {
        Task<List<GenreDto>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre?> GetByNameAsync(string name);
        Task<string> AddAsync(GenreDto newGenre);
        Task<Genre> UpdateAsync(int genreId, GenreDto newGenre);
        Task DeleteAsync(int id);
    }
}
