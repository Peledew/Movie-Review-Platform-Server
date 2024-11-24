using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Services;

namespace MovieReviewerPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRatingsController : ControllerBase
    {
        private readonly IUserRatingService _userRatingService;

        public UserRatingsController(IUserRatingService userRatingService)
        {
            _userRatingService = userRatingService;
        }

        
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add([FromBody] UserRatingDto newRating)
        {
            var resultMessage = await _userRatingService.AddAsync(newRating);

            if (resultMessage == "UserRating Added!")
                return Ok(new { Status = 200, Message = resultMessage });

            return BadRequest(new { Message = resultMessage });

        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserRatingDto>>> GetAll()
        {
            try
            {
                return Ok(await _userRatingService.GetAllAsync());
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "User ratings not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while exporting the user ratings." });
            }
        }

        [HttpGet("rate/{movieId}")]
        [Authorize]
        public async Task<ActionResult<bool>> TryRating(int movieId)
        {
            try
            {
                return Ok(await _userRatingService.TryRatingAsync(movieId));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "Not found: Rating" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "Error while trying to check if it is possible to rate" });
            }
        }



    }
}
