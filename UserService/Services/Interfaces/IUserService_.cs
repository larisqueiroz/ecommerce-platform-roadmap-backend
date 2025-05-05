using UserService.Models.DTO;

namespace UserService.Services.Interfaces
{
    public interface IUserService_
    {
        public Task<List<UserDto>> GetAll();
        public Task<UserDto> GetById(Guid id);
        public Task<UserDto> GetByEmail(string email);
        public Task<UserDto> Create(UserDto userDto);
        public Task<UserDto> Update(UserDto userDto);
        public Task Delete(Guid id);
        public Task<string> Login(UserLoginDto userDto);

    }
}
