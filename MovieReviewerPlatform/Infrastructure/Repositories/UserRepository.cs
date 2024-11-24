using Microsoft.EntityFrameworkCore;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Infrastructure.Context;


namespace MovieReviewerPlatform.Infrastructure.Repositories
{
    // Repositories/UserRepository.cs
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task RegisterAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> IsLoggedInWithRefreshToken(string refreshToken)
        {
            return await _context.Users.AnyAsync(a => a.RefreshToken == refreshToken);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }

}
