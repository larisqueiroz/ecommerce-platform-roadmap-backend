using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserService.Models.DAO;
using UserService.Models.DTO;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class UserService_: IUserService_
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService_(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public List<UserDto> GetAll()
        {
            return _mapper.Map<List<UserDto>>(_userRepository.GetAll());
        }

        public UserDto GetById(Guid id)
        {
            return _mapper.Map<UserDto>(_userRepository.GetById(id));
        }

        public UserDto GetByEmail(string email)
        {
            return _mapper.Map<UserDto>(_userRepository.GetByEmail(email));
        }

        public UserDto Create(UserDto userDto)
        {
            if (userDto == null) {
                throw new ArgumentNullException("user cannot be null");
            }

            if (userDto.Email == null || userDto.Password == null)
            {
                throw new ArgumentNullException("Email and password cannot be null");
            }

            if (_userRepository.GetByEmail(userDto.Email) != null)
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

            return _mapper.Map<UserDto>(_userRepository.Create(user));
        }

        public UserDto Update(UserDto userDto)
        {
            if (userDto.Email == null)
            {
                throw new ArgumentNullException("Email cannot be null");
            }

            var saved = _userRepository.GetByEmail(userDto.Email);
            if (saved == null) {
                throw new ArgumentException("User not found");
            }

            User user = new User
            {
                Id = saved.Id,
                Name = userDto.Name,
                Email = userDto.Email,
                Type = userDto.Type
            };

            return _mapper.Map<UserDto>(_userRepository.Update(user));
        }

        public void Delete(Guid id)
        {
            _userRepository.Delete(id);
        }

        public UserDto Login(UserLoginDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException("user cannot be null");
            }

            if (userDto.Email == null || userDto.Password == null)
            {
                throw new ArgumentNullException("Email and password cannot be null");
            }

            var user = _userRepository.GetByEmail(userDto.Email);
            if (user == null)
            {
                throw new BadHttpRequestException("Email or password is wrong");
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Hash, userDto.Password) == PasswordVerificationResult.Failed) {
                throw new BadHttpRequestException("Wrong email or password");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
