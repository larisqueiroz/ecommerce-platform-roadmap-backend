using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTO;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Authorize("Users")]
    [Route("/scalar/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService_ _userService;
        public UserController(ILogger<UserController> logger, IUserService_ userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Authorize("Administrator")]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return new BadRequestResult();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById([FromQuery] Guid id)
        {
            try
            {
                var user = await _userService.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with id {id}", id);
                return new BadRequestResult();
            }
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with email {email}", email);
                return new BadRequestResult();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserDto userDto)
        {
            try
            {
                var user = await _userService.Create(userDto);
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error creating user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return new BadRequestResult();
            }
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> Update([FromBody] UserDto userDto)
        {
            try
            {
                var user = await _userService.Update(userDto);
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error updating user");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error updating user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return new BadRequestResult();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await _userService.Delete(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return new BadRequestResult();
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] UserLoginDto userDto)
        {
            try
            {
                var token = await _userService.Login(userDto);
                return Ok(token);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error with empty values");
                return BadRequest(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogError(ex, "Error registering user");
                return new BadRequestResult();
            }
        }
    }
}
