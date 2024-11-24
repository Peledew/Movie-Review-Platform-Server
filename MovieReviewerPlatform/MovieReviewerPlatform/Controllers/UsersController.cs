using Microsoft.AspNetCore.Mvc;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = MovieReviewerPlatform.Contracts.Interfaces.IAuthorizationService;
namespace MovieReviewerPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        public UsersController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LogInDto loginUserObj)
        {
            if (loginUserObj == null)
                return BadRequest();

            var user = await _userService.AuthenticateAsync(loginUserObj);

            if (user == null)
                return NotFound(new { Message = "User not found!" });


            return Ok(await _authorizationService.ManageTokensAsync(user));

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userObj)
        {
            var resultMessage = await _userService.RegisterAsync(userObj);

            if (resultMessage == "User Added!")
                return Ok(new { Status = 200, Message = resultMessage });

            return BadRequest(new { Message = resultMessage });

        }

        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetAllAsync()
        {
            return Ok(await _userService.GetAllAsync());
        }

        
        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            var token = await _authorizationService.RefreshAsync(tokenApiDto);
            if(token.AccessToken.Equals("invalid"))
                return BadRequest("Invalid Request");
            else
                return Ok(token);
        }



    }
}
