namespace UserService.Models.DAO;

public class PaymentData: Base
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public float Value { get; set; }
    public string PaymentToken { get; set; }
}