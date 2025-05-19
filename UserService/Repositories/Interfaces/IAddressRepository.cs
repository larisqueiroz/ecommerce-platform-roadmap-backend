using UserService.Models.DAO;

namespace UserService.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        public Task<List<Address>> GetByUser(Guid id);
        public Task<Address> GetById(Guid id);
        public Task<Address> Create(Address address);
        public Task<Address> Update(Address address);
        public Task Delete(Guid id);
    }
}
