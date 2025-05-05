using UserService.Models.DAO;

namespace UserService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAll();
        public Task<User> GetById(Guid id);
        public Task<User> GetByEmail(string email);
        public Task<User> Create(User user);
        public Task<User> Update(User user);
        public Task Delete(Guid id);
    }
}
