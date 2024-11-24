using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.IdentityModel.Tokens;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public MovieService(IMovieRepository movieRepository, IMapper mapper, IGenreService genreService, IBlobStorageService blobStorageService)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _genreService = genreService;
            _blobStorageService = blobStorageService;
        }

        public async Task<string> AddAsync(MovieDto newMovie, IFormFile? image)
        {
            if (newMovie == null)
                return "Invalid data for movie.";

            var movie = _mapper.Map<Movie>(newMovie);
            var genre = await _genreService.GetByIdAsync(movie.Genre.Id);
            movie.Genre = genre;

            if (image != null)
            {
                var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imageUrl = await _blobStorageService.UploadFileAsync("movies", imageFileName, image.OpenReadStream());
                movie.ImageUrl = imageUrl;
            }

            await _movieRepository.AddAsync(movie);
            return "Movie Added!";
        }

        public async Task<double> GetAverageRatingBy(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {movieId} not found.");

            if (movie.UserRatings.IsNullOrEmpty())
                return 0.0; 

            return movie.UserRatings.Sum(r => r.Rating) / movie.UserRatings.Count;
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                throw new Exception("Movie not found");
            }

            await _movieRepository.DeleteAsync(movie);
        }

        public async Task<List<MovieDto>> GetAllAsync()
        {

            return _mapper.Map<List<MovieDto>>(await _movieRepository.GetAllAsync());

        }

        public async Task<List<MovieDto>> GetByGenreIdAsync(int genreId)
        {
            return _mapper.Map<List<MovieDto>>(await _movieRepository.GetByGenreIdAsync(genreId));
        }

        public async Task<MovieDto?> GetByIdAsync(int id)
        {
            var movie = _mapper.Map<MovieDto>(await _movieRepository.GetByIdAsync(id));
            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {id} was not found.");
            }

            return movie;
        }

        public async Task<List<MovieDto>> GetBySearchParametersAsync(int? releaseYear, string? title, int? genreId)
        {
            return _mapper.Map<List<MovieDto>>(await _movieRepository.GetBySearchParametersAsync(releaseYear, title, genreId));
        }

        public async Task<MovieDto> UpdateAsync(int movieId, MovieDto newMovie)
        {
            var oldMovie = await _movieRepository.GetByIdAsync(movieId);
            if (oldMovie == null)
            {
                throw new Exception("Movie not found.");
            }
            _mapper.Map(newMovie, oldMovie);

            await _movieRepository.SaveChangesAsync();
            return _mapper.Map<MovieDto>(oldMovie);
        }

        public async Task<string> DeleteMovieImageByAsync(int movieId)
        {
            // Fetch the movie from the database
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
            {
                return "Movie not found.";
            }

            if (string.IsNullOrEmpty(movie.ImageUrl))
            {
                return "Movie does not have an associated image.";
            }

            // Extract the blob name from the ImageUrl
            var blobName = movie.ImageUrl.Split('/').Last();

            // Delete the image from Azure Blob Storage
            var isDeleted = await _blobStorageService.DeleteFileAsync("movies", blobName);

            if (!isDeleted)
            {
                return "Image not found in storage.";
            }

            // Remove the ImageUrl from the movie record
            movie.ImageUrl = null;
            await UpdateAsync(movie.Id, _mapper.Map<MovieDto>(movie));

            return "Image deleted successfully!";
        }

        public async Task<string> UpdateImageAsync(int movieId, IFormFile newImage)
        {
            // Fetch the movie by ID
            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie == null)
            {
                throw new Exception("Movie not found.");
            }

            // Extract the blob name from the current ImageUrl
            if (!string.IsNullOrEmpty(movie.ImageUrl))
            {
                var uri = new Uri(movie.ImageUrl);
                var blobName = Path.GetFileName(uri.LocalPath);

                // Delete the old image
                await _blobStorageService.DeleteFileAsync("movies", blobName);
            }

            // Upload the new image
            var newBlobName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
            var newImageUrl = await _blobStorageService.UploadFileAsync("movies", newBlobName, newImage.OpenReadStream());

            // Update the movie's ImageUrl and save changes
            movie.ImageUrl = newImageUrl;
            await UpdateAsync(movie.Id, _mapper.Map<MovieDto>(movie));

            return newImageUrl;
        }

    }
}
