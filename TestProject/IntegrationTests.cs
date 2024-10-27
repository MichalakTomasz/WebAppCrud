using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net.Http.Headers;

namespace TestProject
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
	{
        private readonly CustomWebApplicationFactory<Program> _factory;

        public IntegrationTests(CustomWebApplicationFactory<Program> factory)
		{
            _factory = factory;
        }

        [Fact]
        public async Task AuthGuestTestPassed()
        {
            var result = await GuestAuthAsync();
            // Assert
            Assert.True(result.authResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProductTestPassed()
        {
            // Arrange
            var result = await GuestAuthAsync();

            var token = result.authResult.Token;
                        
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var getResponse = await client.GetAsync("/product");
            var getStringContent = await getResponse.Content.ReadAsStringAsync();
            var getResponseJson = JsonSerializer.Deserialize<IEnumerable<Product>>(getStringContent);
            
            // Assert
            Assert.True(!getResponseJson.Any());
        }

        [Fact]
        public async Task RegisterUserTestPassed()
        {
            // Act
            NewAppUser newUser = new()
            {
                Email = "newBuyer@o2.pl",
                Password = "1234Abcd!"
            };

            var result = await RegisterAsync(newUser);

            // Assert
            Assert.True(result.registerResult);
        }

        [Fact]
        public async Task InsertProductTestPassed()
        {
            // Arrange
            NewAppUser newUser = new()
            {
                Email = "user@o2.pl",
                Password = "1234Abcd!"
            };

            var client = (await RegisterAsync(newUser)).client;

            Credentials credentials = new ()
            {
                Email = "user@o2.pl",
                Password = "1234Abcd!"
            };

            AuthModel auth = new ()
            {
                Credentials = credentials,
                AuthType = AuthType.LogIn
            };

            var jsonAuth = JsonSerializer.Serialize(auth);
            var authString = new StringContent(jsonAuth, Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
            InputProduct newProduct = new()
            {
                Name = "Telewizor Xiaomi",
                Code = "Tel122",
                Description = "Telewizor w promocji",
                Price = 1340,
                UrlPicture = "https://sklep.tv.pl"
            };

            var jsonProduct = JsonSerializer.Serialize(newProduct);
            var productString = new StringContent(jsonProduct, Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

            // Act
            var response = await client.PostAsync("/auth", authString);
            var responseBody = await response.Content.ReadAsStringAsync();
            var postAuthResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResult>(responseBody);

            var token = postAuthResponse.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var postResponse = await client.PostAsync("/product", productString);
            var getStringContent = await postResponse.Content.ReadAsStringAsync();
            var postResponseProduct = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(getStringContent);
            
            // Assert
            newProduct.Should().BeEquivalentTo(postResponseProduct,
                o => o.Including(s => s.Name)
                .Including(s => s.Code)
                .Including(s => s.UrlPicture)
                .Including(s => s.Price)
                .Including(s => s.Description));
        }

        private async Task<(HttpClient client,AuthResult? authResult)> GuestAuthAsync()
        {

            // Arrange
            Credentials credentials = new ();
            AuthModel auth = new ()
            {
                Credentials = credentials,
                AuthType = AuthType.Guest
            };
            var jsonContent = JsonSerializer.Serialize(auth);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

            // Act            
            var client = _factory.CreateClient();
            var response = await client.PostAsync("/auth", stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResult>(responseBody);

            return (client, jsonResponse);
        }

        private async Task<(HttpClient client, bool registerResult)> RegisterAsync(NewAppUser user)
        {
            var jsonNewUser = JsonSerializer.Serialize(user);
            var stringNewUserContent = new StringContent(jsonNewUser, Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
            var client = _factory.CreateClient();
            var retgisterResponse = await client.PostAsync("/auth/register", stringNewUserContent);
            var registerResponseBody = await retgisterResponse.Content.ReadAsStringAsync();
            var registerJsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(registerResponseBody);

            return (client, registerJsonResponse);
        }
    }
}