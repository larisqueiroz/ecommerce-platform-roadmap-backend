using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models.DTO;

namespace UserService.Tests.Utils
{
    
    public class LoginHelper
    {
        public HttpClient _client;

        public LoginHelper(IntegrationTestWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        public string GetToken()
        {
            var login = new UserLoginDto
            {
                Email = "admin@email.com",
                Password = "Admin@@123"
            };

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var result = _client.PostAsync("/scalar/v1/users/login", content).Result;

            var token = result.Content.ReadAsStringAsync().Result;

            return token;
        }
    }
}
