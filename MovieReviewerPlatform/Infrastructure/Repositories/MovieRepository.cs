using Microsoft.EntityFrameworkCore;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Infrastructure.Context;

namespace MovieReviewerPlatform.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .Include(c => c.UserComments)
                .ToListAsync();
        }

        public async Task<List<Movie>> GetByGenreIdAsync(int genreId)
        {
            return await _context.Movies
                .Include(m => m.Genre) 
                .Where(x => x.Genre.Id == genreId) 
                .ToListAsync();
        }

        public async Task<List<Movie>> GetBySearchParametersAsync(int? releaseYear, string? title, int? genreId)
        {
            var query = _context.Movies.Include(m => m.Genre).AsQueryable();

            // Filter by releaseYear if provided (not 0)
            if (releaseYear != 0)
            {
                query = query.Where(x => x.ReleaseDate.Year == releaseYear);
            }

            // Filter by title if provided (not an empty string or specific placeholder)
            if (!string.IsNullOrEmpty(title) && title != "```")
            {
                query = query.Where(x => x.Title.Contains(title));
            }

            // Filter by genreId if provided (not 0)
            if (genreId != 0)
            {
                query = query.Where(x => x.Genre.Id == genreId);  // Assuming Genre is a related object and has an Id
            }

            return await query.ToListAsync();
        }






        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .Include(r => r.UserRatings)
                .Include(c => c.UserComments)
                    .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
