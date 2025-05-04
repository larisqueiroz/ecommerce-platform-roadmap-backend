using UserService.Models.DTO;

namespace UserService.Services.Interfaces
{
    public interface IUserService_
    {
        public List<UserDto> GetAll();
        public UserDto GetById(Guid id);
        public UserDto GetByEmail(string email);
        public UserDto Create(UserDto userDto);
        public UserDto Update(UserDto userDto);
        public void Delete(Guid id);
        public UserDto Login(UserLoginDto userDto);

    }
}
