using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using System.Security.Policy;
using UserService.Data;
using UserService.Models.DAO;
using UserService.Models.DTO;
using UserService.Repositories.Implementations;
using UserService.Repositories.Interfaces;
using UserService.Services.Implementations;
using UserService.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenApi();

builder.Services.AddDbContext<UserServiceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("USERSERVICE")));

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService_, UserService_>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserServiceContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    if (context.Users.FirstOrDefault(u => u.Type == UserService.Enum.UserType.ADMIN) == null)
    {
        var userAdmin = new User()
        {
            Email = "admin@email.com",
            Name = "Admin",
            Type = UserService.Enum.UserType.ADMIN,
            Active = true,

        };
        userAdmin.Hash = new PasswordHasher<User>().HashPassword(userAdmin, "Admin@@123");
        context.Users.Add(userAdmin);

        context.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();