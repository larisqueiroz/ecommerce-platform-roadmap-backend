using Docker.DotNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Text;
using UserService.Models.DAO;

namespace UserService.Tests
{
    public class UserServiceTests: IClassFixture<IntegrationTestWebAppFactory>
    {

        private readonly HttpClient _client;
        private readonly Uri requestUri;
        public UserServiceTests(IntegrationTestWebAppFactory factory)
        {
            _client = factory.CreateClient();

            requestUri = new UriBuilder(Uri.UriSchemeHttp, "localhost", factory.GetPort(), "scalar/v1").Uri;
        }


        [Fact]
        public async Task CreateMeter_ShouldCreateProductAsync()
        {
            var user = new User
            {
                Email = "test@email.com",
                Name = "Test",
                Hash = new PasswordHasher<User>().HashPassword(new User(), "test")
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/scalar/v1/api/users", content);

            var body = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<User>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(createdUser);

        }
    }
}
