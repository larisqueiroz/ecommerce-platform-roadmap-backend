using UserService.Models.DTO;

namespace UserService.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<AddressDto>> GetByUser(Guid id);
        Task<AddressDto> GetById(Guid id);
        Task<AddressDto> Create(AddressDto addressDto);
        Task<AddressDto> Update(AddressDto addressDto);
        Task Delete(Guid id);
    }
}
