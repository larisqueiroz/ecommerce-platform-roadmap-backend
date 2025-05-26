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
        public async Task CreateMeter_ShouldCreateProduct()
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

        [Fact]
        public async Task GetAll_ShouldReturnUsers()
        {
            var token = _loginHelper.GetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "/scalar/v1/users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _loginHelper._client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserDto>>(body);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(users);
        }

        [Fact]
        public async Task GetById_ShouldReturnUser()
        {
            var token = _loginHelper.GetToken();

            var user = new UserDto()
            {
                Email = "testgetbyid@email.com",
                Password = "testgetbyid@123",
                Name = "TestGetById",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var saved = await _loginHelper._client.PostAsync("/scalar/v1/users", content);
            var savedContent = saved.Content.ReadAsStringAsync().Result;
            var savedUser = JsonConvert.DeserializeObject<UserDto>(savedContent);

            var request = new HttpRequestMessage(HttpMethod.Get, "/scalar/v1/users/by-id?id=" + savedUser.Id);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _loginHelper._client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            var userById = JsonConvert.DeserializeObject<UserDto>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userById);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnUser()
        {
            var token = _loginHelper.GetToken();

            var user = new UserDto()
            {
                Email = "testget@email.com",
                Password = "testget@123",
                Name = "TestGet",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var saved = await _loginHelper._client.PostAsync("/scalar/v1/users", content);
            var savedContent = saved.Content.ReadAsStringAsync().Result;
            var savedUser = JsonConvert.DeserializeObject<UserDto>(savedContent);

            var request = new HttpRequestMessage(HttpMethod.Get, "/scalar/v1/users/by-email?email=" + savedUser.Email);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _loginHelper._client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            var userByEmail = JsonConvert.DeserializeObject<UserDto>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userByEmail);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnUser()
        {
            var token = _loginHelper.GetToken();

            var user = new UserDto()
            {
                Email = "testedit@email.com",
                Password = "testedit@123",
                Name = "TestEdit",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var saved = await _loginHelper._client.PostAsync("/scalar/v1/users", content);
            var json = await saved.Content.ReadAsStringAsync();

            var savedUser = JsonConvert.DeserializeObject<UserDto>(json);
            savedUser.Name = "TestEditUpdated";

            var request = new HttpRequestMessage(HttpMethod.Put, "/scalar/v1/users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonConvert.SerializeObject(savedUser), Encoding.UTF8, "application/json");

            var response = await _loginHelper._client.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();
            var updatedUser = JsonConvert.DeserializeObject<UserDto>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(updatedUser);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturn204NoContent()
        {
            var token = _loginHelper.GetToken();

            var user = new UserDto()
            {
                Email = "testdelete@email.com",
                Password = "testedelete@123",
                Name = "TestDelete",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var saved = await _loginHelper._client.PostAsync("/scalar/v1/users", content);
            var json = await saved.Content.ReadAsStringAsync();

            var savedUser = JsonConvert.DeserializeObject<UserDto>(json);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/scalar/v1/users?id={savedUser.Id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _loginHelper._client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
