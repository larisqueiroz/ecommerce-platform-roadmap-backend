namespace UserService.Models.DAO;

public class Base
{
    public Guid Id { get; set; } = new Guid();
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}