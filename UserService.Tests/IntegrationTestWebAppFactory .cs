using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using UserService.Data;
using Program = UserService;

namespace UserService.Tests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<ProgramPartial>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithPortBinding(8080, true)
            .Build();


        private string _connectionString;

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();
            _connectionString = _postgresContainer.GetConnectionString();
        }

        public Task DisposeAsync() => _postgresContainer.DisposeAsync().AsTask();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<UserServiceContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<UserServiceContext>(options =>
                {
                    options.UseNpgsql(_connectionString);
                });
            });
        }

        public ushort GetPort()
        {
            return _postgresContainer.GetMappedPublicPort(8080);
        }

    }
}
