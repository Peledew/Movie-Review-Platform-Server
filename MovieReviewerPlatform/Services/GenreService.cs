using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<string> AddAsync(GenreDto newGenre)
        {
            if (newGenre == null)
                return "Invalid data for genre.";

            var genre = _mapper.Map<Genre>(newGenre);
            await _genreRepository.AddAsync(genre);

            return "Genre Added!";
        }
        public async Task DeleteAsync(int id)
        {
            var genre = await GetByIdAsync(id);
            if(genre == null)
            {
                throw new Exception("Genre not found");
            }

             await _genreRepository.DeleteAsync(genre);
        }

        public async Task<List<GenreDto>> GetAllAsync()
        {
            return _mapper.Map<List<GenreDto>>(await _genreRepository.GetAllAsync());
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                throw new KeyNotFoundException($"Genre with ID {id} was not found.");
            }

            return genre;
        }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            return await _genreRepository.GetByNameAsync(name);
        }

        public async Task<Genre> UpdateAsync(int genreId, GenreDto newGenre)
        {
            var oldGenre = await GetByIdAsync(genreId);
            if (oldGenre == null)
            {
                // Handle the case where the genre doesn't exist
                throw new Exception("Genre not found.");
            }
            _mapper.Map(newGenre, oldGenre);

            await _genreRepository.SaveChangesAsync();

            return oldGenre;
        }
    }
}
