using CatalogService.Data;
using Scalar.AspNetCore;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using CatalogService.Models.DAO;
using CatalogService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CatalogServiceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("CATALOGSERVICE")));

builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogServiceContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    if (!context.Categories.Any())
    {
        List<Category> categories = new List<Category>
        {
            new Category { Name = "Shirts" },
            new Category { Name = "T-Shirts" },
            new Category { Name = "Dresses"},
            new Category { Name = "Beach Wear"},
            new Category { Name = "Shoes"},
            new Category { Name = "Active Wear"},
            new Category { Name = "Trousers"},
            new Category { Name = "Shorts"},
            new Category { Name = "Bags"},
            new Category { Name = "Jewelry"}
        };

        context.Categories.AddRange(categories);
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
