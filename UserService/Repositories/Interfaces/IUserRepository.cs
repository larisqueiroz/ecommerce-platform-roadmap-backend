using UserService.Models.DAO;

namespace UserService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public User GetById(Guid id);
        public User GetByEmail(string email);
        public User Create(User user);
        public User Update(User user);
        public void Delete(Guid id);
    }
}
