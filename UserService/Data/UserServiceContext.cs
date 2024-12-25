using Microsoft.EntityFrameworkCore;
using UserService.Models.DAO;

namespace UserService.Data;

public class UserServiceContext: DbContext
{
    public UserServiceContext(DbContextOptions<UserServiceContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public DbSet<User> Users { get; set; } 
    public DbSet<Address> Addresses { get; set; } 
    public DbSet<PaymentData> PaymentDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasAlternateKey(u => u.Email);
        
    }
}