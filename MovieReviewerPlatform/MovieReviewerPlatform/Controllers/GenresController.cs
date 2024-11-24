using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;

namespace MovieReviewerPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] GenreDto newGenre)
        {
            var resultMessage = await _genreService.AddAsync(newGenre);

            if (resultMessage == "Genre Added!")
                return Ok(new { Status = 200, Message = resultMessage });

            return BadRequest(new {Message = resultMessage});
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBy(int id)
        {
            try
            {
                await _genreService.DeleteAsync(id);
                return Ok(new { Status = 200, Message = "Genre has been deleted successfully!" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Genre not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while deleting the genre." });
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GenreDto>>> GetAll()
        {
            try
            {
                return Ok(await _genreService.GetAllAsync());
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Genres not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while exporting the genres." });
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBy(int id, [FromBody] GenreDto newGenre)
        {
            try
            {
                return Ok(await _genreService.UpdateAsync(id, newGenre));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Genre not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while updating the genre." });
            }
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _genreService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Genre not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while updating the genre." });
            }
        }
    }
}
