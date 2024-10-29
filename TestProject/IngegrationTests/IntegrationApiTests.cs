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
using TestProject.TestHelpers;

namespace TestProject.IngegrationTests
{
    [Collection("Sequential")]
    public class IntegrationApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public IntegrationApiTests(CustomWebApplicationFactory<Program> factory)
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
            var getProductsResponse = await client.GetAsync("/product");
            var productsResponse = await getProductsResponse.ConvertTo<IEnumerable<Product>>();
            // Assert
            Assert.True(!productsResponse.Any());
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

            Credentials credentials = new()
            {
                Email = "user@o2.pl",
                Password = "1234Abcd!"
            };

            AuthModel auth = new()
            {
                Credentials = credentials,
                AuthType = AuthType.LogIn
            };

            InputProduct newProduct = new()
            {
                Name = "Telewizor Xiaomi",
                Code = "Tel122",
                Description = "Telewizor w promocji",
                Price = 1340,
                UrlPicture = "https://sklep.tv.pl"
            };

            // Act
            var authResponse = await client.PostAsync("/auth", auth.ToContent());
            var token = (await authResponse.ConvertTo<AuthResult>()).Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var productsResponse = await client.PostAsync("/product", newProduct.ToContent());
            var responseProduct = await productsResponse.ConvertTo<Product>();
            // Assert
            newProduct.Should().BeEquivalentTo(responseProduct,
                o => o.Including(s => s.Name)
                .Including(s => s.Code)
                .Including(s => s.UrlPicture)
                .Including(s => s.Price)
                .Including(s => s.Description));
        }

        private async Task<(HttpClient client, AuthResult? authResult)> GuestAuthAsync()
        {

            // Arrange
            Credentials credentials = new();
            AuthModel auth = new()
            {
                Credentials = credentials,
                AuthType = AuthType.Guest
            };
            var jsonContent = JsonSerializer.Serialize(auth);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

            // Act            
            var client = _factory.CreateClient();
            var authResponse = await client.PostAsync("/auth", stringContent);
            var authResult = await authResponse.ConvertTo<AuthResult>();
            return (client, authResult);
        }

        private async Task<(HttpClient client, bool registerResult)> RegisterAsync(NewAppUser user)
        {
            var userContent = user.ToContent();
            var client = _factory.CreateClient();
            var registerResponse = await client.PostAsync("/auth/register", userContent);
            var registerResult = await registerResponse.ConvertTo<bool>();
            return (client, registerResult);
        }
    }
}