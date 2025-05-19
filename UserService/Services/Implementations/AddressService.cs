using AutoMapper;
using UserService.Models.DAO;
using UserService.Models.DTO;
using UserService.Repositories.Implementations;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<List<AddressDto>> GetByUser(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(id));

            var addresses = await _addressRepository.GetByUser(id);
            return _mapper.Map<List<AddressDto>>(addresses);
        }

        public async Task<AddressDto> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(id));
            var address = await _addressRepository.GetById(id);
            return _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> Create(AddressDto addressDto)
        {
            if (addressDto == null)
                throw new ArgumentNullException(nameof(addressDto), "Address cannot be null.");

            var address = _mapper.Map<Address>(addressDto);
            var createdAddress = await _addressRepository.Create(address);
            return _mapper.Map<AddressDto>(createdAddress);
        }

        public async Task<AddressDto> Update(AddressDto addressDto)
        {
            if (addressDto == null)
                throw new ArgumentNullException(nameof(addressDto), "Address cannot be null.");
            if (addressDto.Id == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(addressDto.Id));

            var address = await _addressRepository.GetById((Guid)addressDto.Id);
            if (address == null)
                throw new ArgumentException("Address not found.");

            address.Street = addressDto.Street;
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.ZipCode = addressDto.ZipCode;
            address.UpdatedAt = DateTime.UtcNow;
            address.Neighborhood = addressDto.Neighborhood;
            address.Country = addressDto.Country;

            var updatedAddress = await _addressRepository.Update(address);
            return _mapper.Map<AddressDto>(updatedAddress);
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(id));
            var address = await _addressRepository.GetById(id);
            if (address == null)
                throw new ArgumentException("Address not found.");
            await _addressRepository.Delete(id);
        }
    }
}
