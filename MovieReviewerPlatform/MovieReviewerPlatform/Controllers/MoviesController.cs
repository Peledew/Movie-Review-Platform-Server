using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;

namespace MovieReviewerPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] MovieDto newMovie, IFormFile? image)
        {
            var resultMessage = await _movieService.AddAsync(newMovie, image);

            if (resultMessage == "Movie Added!")
                return Ok(new { Status = 200, Message = resultMessage });

            return BadRequest(new { Message = resultMessage });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBy(int id)
        {
            try
            {
                await _movieService.DeleteAsync(id);
                return Ok(new { Status = 200, Message = "Movie has been deleted successfully!" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movie not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while deleting the movie." });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<MovieDto>>> GetAll()
        {
            try
            {
                return Ok(await _movieService.GetAllAsync());
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movies not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while exporting the movies." });
            }
        }

        [HttpGet("genre/{id}")]
        [Authorize]
        public async Task<ActionResult<List<MovieDto>>> GetByGenreId(int id)
        {
            try
            {
                return Ok(await _movieService.GetByGenreIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movies not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while exporting the movies." });
            }
        }

        [HttpGet("releaseYear/title/genre")]
        [Authorize]
        public async Task<ActionResult<List<MovieDto>>> GetBySearchParameters([FromQuery] int? releaseYear, [FromQuery] string? title, [FromQuery] int? genreId)
        {
            try
            {
                return Ok(await _movieService.GetBySearchParametersAsync(releaseYear, title, genreId));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movies not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while exporting the movies." });
            }
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBy(int id, [FromBody] MovieDto newMovie)
        {
            try
            {
                return Ok(await _movieService.UpdateAsync(id, newMovie));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movie not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while updating the movie." });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByMovieId(int id)
        {
            try
            {
                return Ok(await _movieService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movie not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while updating the movie." });
            }
        }

        [HttpGet("avgrating/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAverageRatingBy(int id)
        {
            try
            {
                return Ok(await _movieService.GetAverageRatingBy(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Movie not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while updating the movie." });
            }
        }

        [HttpPut("update-image/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile newImage)
        {
            if (newImage == null || newImage.Length == 0)
            {
                return BadRequest(new { Message = "Invalid image file." });
            }

            try
            {
                var newImageUrl = await _movieService.UpdateImageAsync(id, newImage);
                return Ok(new { Status = 200, Message = "Image updated successfully.", ImageUrl = newImageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}/delete-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovieImage(int id)
        {
            var resultMessage = await _movieService.DeleteMovieImageByAsync(id);

            if (resultMessage == "Image deleted successfully!")
            {
                return Ok(new { Message = resultMessage });
            }

            return BadRequest(new { Message = resultMessage });
        }


    }
}
