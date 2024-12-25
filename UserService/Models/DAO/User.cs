using UserService.Enum;
using UserService.Models.DTO;

namespace UserService.Models.DAO;

public class User: Base
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Hash { get; set; }
    public UserType Type { get; set; }
    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<PaymentData> PaymentDatas { get; set; } = new List<PaymentData>();
}