using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace MovieReviewerPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : throw new Exception("User not authenticated");
        }

        public async Task<string> RegisterAsync(UserDto userData)
        {
            if (userData == null)
                return "Invalid user object.";

            userData.Role = "User";

            var user = _mapper.Map<User>(userData);
            await _userRepository.RegisterAsync(user);

            return "User Added!";
        }

        public async Task<User> AuthenticateAsync(LogInDto user)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser == null)
            {
                return null; // User not found
            }

            if (!ValidatePasswordAsync(user.Password, existingUser))
            {
                return null;
            }

            return existingUser;
        }

        public async Task<User> GetBy(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<bool> isRefreshTokenValidAsync(string RefreshToken)
        {
            return await _userRepository.IsLoggedInWithRefreshToken(RefreshToken);
        }

        public async Task SaveChangesAsync()
        {
            await _userRepository.SaveChangesAsync();
        }

        public bool ValidatePasswordAsync(string password, User user)
        {
            return (user.Password == password);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }

}
