using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models.DAO;
using UserService.Repositories.Interfaces;

namespace UserService.Repositories.Implementations
{
    public class AddressRepository: IAddressRepository
    {
        private readonly UserServiceContext _context;
        public AddressRepository(UserServiceContext context)
        {
            _context = context;
        }
        
        public async Task<List<Address>> GetByUser(Guid id)
        {
            return await _context.Addresses.Where(a => a.UserId == id && a.Active).ToListAsync();
        }

        public async Task<Address> GetById(Guid id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.Active);
        }

        public async Task<Address> Create(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> Update(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task Delete(Guid id)
        {
            var address = await GetById(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }

}
