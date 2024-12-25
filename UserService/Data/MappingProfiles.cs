using System.Data.Entity.Core.Mapping;
using AutoMapper;
using UserService.Models.DAO;
using UserService.Models.DTO;

namespace UserService.Data;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();

        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();

        CreateMap<PaymentData, PaymentDataDto>();
        CreateMap<PaymentDataDto, PaymentData>();
    }
}