using UserService.Enum;

namespace UserService.Models.DAO;

public class PaymentData: Base
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public PaymentType Type { get; set; }
    public string PaymentToken { get; set; }
}