using Microsoft.EntityFrameworkCore;
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

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(Guid id)
        {
            _context.Users.Remove(await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id));
            await _context.SaveChangesAsync();
        }

    }
}
