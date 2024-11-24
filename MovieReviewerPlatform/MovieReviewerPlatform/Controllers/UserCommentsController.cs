using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;

namespace MovieReviewerPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommentsController : ControllerBase
    {
        private readonly IUserCommentService _userCommentService;

        public UserCommentsController(IUserCommentService userCommentService)
        {
            _userCommentService = userCommentService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add([FromBody] UserCommentDto newComment)
        {
            var resultMessage = await _userCommentService.AddAsync(newComment);

            if (resultMessage == "UserComment Added!")
                return Ok(new { Status = 200, Message = resultMessage });

            return BadRequest(new { Message = resultMessage });

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBy(int id)
        {
            try
            {
                await _userCommentService.DeleteAsync(id);
                return Ok(new { Status = 200, Message = "UserComment has been deleted successfully!" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "UserComment not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while deleting the UserComment." });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserCommentDto>>> GetAll()
        {
            try
            {
                return Ok(await _userCommentService.GetAllAsync());
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Status = 404, Message = "User comments not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Status = 500, Message = "An error occurred while exporting the user comments." });
            }
        }


    }
}
