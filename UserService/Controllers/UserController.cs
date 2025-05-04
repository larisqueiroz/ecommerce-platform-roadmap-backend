using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTO;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService_ _userService;
        public UserController(ILogger<UserController> logger, IUserService_ userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return new BadRequestResult();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromQuery] Guid id)
        {
            try
            {
                var user = _userService.GetById(id);
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

        [HttpGet("{email}")]
        public IActionResult GetById([FromQuery] string id)
        {
            try
            {
                var user = _userService.GetByEmail(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with email {email}", id);
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserDto userDto)
        {
            try
            {
                var user = _userService.Create(userDto);
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
        public IActionResult Update([FromBody] UserDto userDto)
        {
            try
            {
                var user = _userService.Update(userDto);
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
        public IActionResult Delete([FromQuery] Guid id)
        {
            try
            {
                _userService.Delete(id);
                return Ok();
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userDto)
        {
            try
            {
                var user = _userService.Login(userDto);
                return Ok(user);
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
