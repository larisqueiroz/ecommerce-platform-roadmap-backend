using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Policy;
using System.Text;
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Token"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireRole("ADMIN")
        .RequireAuthenticatedUser().Build());

    options.AddPolicy("Users", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireRole("ADMIN","USER","OPERATOR")
        .RequireAuthenticatedUser().Build());

    options.AddPolicy("Operators", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireRole("OPERATOR", "ADMIN")
        .RequireAuthenticatedUser().Build());

    var authorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

    authorizationPolicyBuilder = authorizationPolicyBuilder.RequireAuthenticatedUser();

    options.DefaultPolicy = authorizationPolicyBuilder.Build();
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService_, UserService_>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserServiceContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    if (!context.Users.Any())
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

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();