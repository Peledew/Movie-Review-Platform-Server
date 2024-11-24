using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllAsync();
        
        Task<Genre?> GetByIdAsync(int id);
        
        Task AddAsync(Genre genre);        
        Task DeleteAsync(Genre genre);
        Task SaveChangesAsync();
        Task<Genre?> GetByNameAsync(string name);
        
    }
}
