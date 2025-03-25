using UserService.Models.DAO;

namespace UserService.Models.DTO;

public class PaymentDataDto: BaseDto
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public float Value { get; set; }
}