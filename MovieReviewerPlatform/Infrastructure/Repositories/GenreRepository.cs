using Microsoft.EntityFrameworkCore;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Infrastructure.Context;

namespace MovieReviewerPlatform.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Genre genre)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}