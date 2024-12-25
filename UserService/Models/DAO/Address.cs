namespace UserService.Models.DAO;

public class Address: Base
{
    public Guid UserId { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string? Neighborhood { get; set; }
}