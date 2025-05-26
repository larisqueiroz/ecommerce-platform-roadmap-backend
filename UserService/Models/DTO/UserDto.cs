using UserService.Enum;

namespace UserService.Models.DTO;

public class UserDto: BaseDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public UserType Type { get; set; }
    public List<AddressDto>? Addresses { get; set; } = new List<AddressDto>();
    public List<PaymentDataDto>? PaymentDatas { get; set; } = new List<PaymentDataDto>();
}