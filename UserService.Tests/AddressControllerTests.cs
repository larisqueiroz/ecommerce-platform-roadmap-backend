using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserService.Models.DTO;
using UserService.Tests.Utils;

namespace UserService.Tests
{
    public class AddressControllerTests: IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly LoginHelper _loginHelper;
        private AddressDto address = new AddressDto()
        {
            City = "TestCity",
            State = "TestState",
            Neighborhood = "TestNeighborhood",
            Street = "TestStreet",
            Number = "123",
            ZipCode = "12345678",
            Country = "TestCountry",
        };

        public AddressControllerTests(IntegrationTestWebAppFactory factory)
        {
            _loginHelper = new LoginHelper(factory);
        }

        [Fact]
        public async Task CreateAddress_ShouldCreateAddress()
        {
            var user = new UserDto()
            {
                Email = "testcreate@email.com",
                Password = "testcreate@123",
                Name = "TestCreate",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _loginHelper._client.PostAsync("/scalar/v1/users", content);

            var body = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserDto>(body);

            address.UserId = (Guid)createdUser.Id;

            var token = _loginHelper.GetToken();

            var request = new HttpRequestMessage(HttpMethod.Post, "/scalar/v1/addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");

            var responseAddress = await _loginHelper._client.SendAsync(request);
            var addressBody = await responseAddress.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressDto>(addressBody);

            Assert.Equal(HttpStatusCode.OK, responseAddress.StatusCode);
            Assert.NotNull(createdAddress);

        }

        [Fact]
        public async Task GetById_ShouldReturnAddress()
        {
            // create user 

            var user = new UserDto()
            {
                Email = "testgetbyid@email.com",
                Password = "testgetbyid@123",
                Name = "TestById",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _loginHelper._client.PostAsync("/scalar/v1/users", content);

            var body = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserDto>(body);

            address.UserId = (Guid)createdUser.Id;

            var token = _loginHelper.GetToken();

            // create address
            var request = new HttpRequestMessage(HttpMethod.Post, "/scalar/v1/addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");

            var responseAddress = await _loginHelper._client.SendAsync(request);
            var addressBody = await responseAddress.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressDto>(addressBody);

            // get address

            var requestGet = new HttpRequestMessage(HttpMethod.Get, "/scalar/v1/addresses/by-id?id=" + createdAddress.Id);
            requestGet.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var responseGet = await _loginHelper._client.SendAsync(requestGet);
            var addressGet = await responseAddress.Content.ReadAsStringAsync();
            var gottenAddress = JsonConvert.DeserializeObject<AddressDto>(addressGet);

            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.NotNull(gottenAddress);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnAddresses()
        {
            // create user 

            var user = new UserDto()
            {
                Email = "testgetbyuserid@email.com",
                Password = "testgetbyuserid@123",
                Name = "TestByUserId",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _loginHelper._client.PostAsync("/scalar/v1/users", content);

            var body = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserDto>(body);

            address.UserId = (Guid)createdUser.Id;

            var token = _loginHelper.GetToken();

            // create address
            var request = new HttpRequestMessage(HttpMethod.Post, "/scalar/v1/addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");

            var responseAddress = await _loginHelper._client.SendAsync(request);
            var addressBody = await responseAddress.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressDto>(addressBody);

            // get address

            var requestGet = new HttpRequestMessage(HttpMethod.Get, "/scalar/v1/addresses/by-userid?id=" + createdUser.Id);
            requestGet.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var responseGet = await _loginHelper._client.SendAsync(requestGet);
            var addressesGet = await responseAddress.Content.ReadAsStringAsync();
            var gottenAddresses = JsonConvert.DeserializeObject<AddressDto>(addressesGet);

            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.NotNull(gottenAddresses);
        }

       

        [Fact]
        public async Task UpdateAddress_ShouldReturnAddress()
        {
            var token = _loginHelper.GetToken();

            // save user
            var user = new UserDto()
            {
                Email = "testeditaddress@email.com",
                Password = "testeditaddress@123",
                Name = "TestEditAddress",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var saved = await _loginHelper._client.PostAsync("/scalar/v1/users", content);
            var json = await saved.Content.ReadAsStringAsync();
            var savedUser = JsonConvert.DeserializeObject<UserDto>(json);

            // create address
            address.UserId = (Guid)savedUser.Id;
            var request = new HttpRequestMessage(HttpMethod.Post, "/scalar/v1/addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");

            var responseAddress = await _loginHelper._client.SendAsync(request);
            var addressBody = await responseAddress.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressDto>(addressBody);

            // edit address

            createdAddress.Street = "UpdatedStreet";
            createdAddress.Number = "456";

            var requestEdit = new HttpRequestMessage(HttpMethod.Put, "/scalar/v1/addresses");
            var editedContent = new StringContent(JsonConvert.SerializeObject(createdAddress), Encoding.UTF8, "application/json");
            requestEdit.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requestEdit.Content = editedContent;

            var responseEdit = await _loginHelper._client.SendAsync(requestEdit);
            var addressesEdit = await responseEdit.Content.ReadAsStringAsync();
            var editedAddress = JsonConvert.DeserializeObject<AddressDto>(addressesEdit);

            Assert.Equal(HttpStatusCode.OK, responseEdit.StatusCode);
            Assert.NotNull(editedAddress);
        }

        [Fact]
        public async Task DeleteAddress_ShouldReturn204NoContent()
        {
            var token = _loginHelper.GetToken();

            // save user
            var user = new UserDto()
            {
                Email = "testedelete@email.com",
                Password = "testedelete@123",
                Name = "TestDeleteAddress",
                Type = Enum.UserType.USER,
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var saved = await _loginHelper._client.PostAsync("/scalar/v1/users", content);
            var json = await saved.Content.ReadAsStringAsync();
            var savedUser = JsonConvert.DeserializeObject<UserDto>(json);

            // create address
            address.UserId = (Guid)savedUser.Id;
            var request = new HttpRequestMessage(HttpMethod.Post, "/scalar/v1/addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");

            var responseAddress = await _loginHelper._client.SendAsync(request);
            var addressBody = await responseAddress.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressDto>(addressBody);

            // delete address

            var requestDelete = new HttpRequestMessage(HttpMethod.Delete, "/scalar/v1/addresses?id=" + createdAddress.Id);
            requestDelete.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var responseDelete = await _loginHelper._client.SendAsync(requestDelete);

            Assert.Equal(HttpStatusCode.NoContent, responseDelete.StatusCode);
        }
    }
}
