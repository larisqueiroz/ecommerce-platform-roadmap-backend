using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models.DAO;
using UserService.Models.DTO;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class UserService_: IUserService_
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserService_(IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<List<UserDto>> GetAll()
        {
            return _mapper.Map<List<UserDto>>(await _userRepository.GetAll());
        }

        public async Task<UserDto> GetById(Guid id)
        {
            return _mapper.Map<UserDto>(await _userRepository.GetById(id));
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            return _mapper.Map<UserDto>(await _userRepository.GetByEmail(email));
        }

        public async Task<UserDto> Create(UserDto userDto)
        {
            if (userDto == null) {
                throw new ArgumentNullException("user cannot be null");
            }

            if (userDto.Email == null || userDto.Password == null)
            {
                throw new ArgumentNullException("Email and password cannot be null");
            }

            if (await _userRepository.GetByEmail(userDto.Email) != null)
            {
                throw new Exception("User with this email already exists");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Type = userDto.Type
            };

            var hasherPassword = new PasswordHasher<User>().HashPassword(user, userDto.Password);

            user.Hash = hasherPassword;

            return _mapper.Map<UserDto>(await _userRepository.Create(user));
        }

        public async Task<UserDto> Update(UserDto userDto)
        {
            if (userDto.Email == null)
            {
                throw new ArgumentNullException("Email cannot be null");
            }

            var saved = await _userRepository.GetByEmail(userDto.Email);
            if (saved == null) {
                throw new ArgumentException("User not found");
            }

            User user = new User
            {
                Id = saved.Id,
                Name = userDto.Name,
                Type = userDto.Type,
                UpdatedAt = DateTime.Now
            };

            return _mapper.Map<UserDto>(await _userRepository.Update(user));
        }

        public async Task Delete(Guid id)
        {
            await _userRepository.Delete(id);
        }

        public async Task<string> Login(UserLoginDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException("user cannot be null");
            }

            if (userDto.Email == null || userDto.Password == null)
            {
                throw new ArgumentNullException("Email and password cannot be null");
            }

            var user = await _userRepository.GetByEmail(userDto.Email);
            if (user == null)
            {
                throw new BadHttpRequestException("Email or password is wrong");
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Hash, userDto.Password) == PasswordVerificationResult.Failed) {
                throw new BadHttpRequestException("Wrong email or password");
            }

            string token = await CreateToken(user);

            return token;
        }

        private async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Type.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Authentication:Token")!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(issuer: _configuration.GetValue<string>("Authentication:Issuer"), 
                audience: _configuration.GetValue<string>("Authentication:Audience"),
                claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }   
    }
}
