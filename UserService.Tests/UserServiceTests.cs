using Docker.DotNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Text;
using UserService.Models.DAO;
using UserService.Models.DTO;
using UserService.Tests.Utils;

namespace UserService.Tests
{
    public class UserServiceTests: IClassFixture<IntegrationTestWebAppFactory>
    {

        private readonly LoginHelper _loginHelper;
        public UserServiceTests(IntegrationTestWebAppFactory factory)
        {
            _loginHelper = new LoginHelper(factory);
        }


        [Fact]
        public async Task CreateMeter_ShouldCreateProductAsync()
        {
            var user = new UserDto()
            {
                Email = "test@email.com",
                Password = "test@123",
                Name = "Test",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _loginHelper._client.PostAsync("/scalar/v1/users", content);

            var body = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserDto>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(createdUser);

        }

        [Fact]
        public async Task Login_ShouldReturnToken()
        {
            var result = _loginHelper.GetToken();

            Assert.NotNull(result);
        }
    }
}
