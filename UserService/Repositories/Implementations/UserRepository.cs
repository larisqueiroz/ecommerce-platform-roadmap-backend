using UserService.Data;
using UserService.Models.DAO;
using UserService.Repositories.Interfaces;

namespace UserService.Repositories.Implementations
{
    public class UserRepository: IUserRepository
    {
        private readonly UserServiceContext _context;
        public UserRepository(UserServiceContext context) {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public void Delete(Guid id)
        {
            _context.Users.Remove(_context.Users.FirstOrDefault(u => u.Id == id));
            _context.SaveChanges();
        }

    }
}
