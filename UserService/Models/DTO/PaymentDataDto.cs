using UserService.Enum;
using UserService.Models.DAO;

namespace UserService.Models.DTO;

public class PaymentDataDto: BaseDto
{
    public Guid? Id { get; set; }
    public PaymentType Type { get; set; }
    public Guid UserId { get; set; }
}