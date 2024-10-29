using Domain.Models;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestProject.TestHelpers;

namespace TestProject
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class SequentialCollection : ICollectionFixture<SequentialTestFixture>
    {
    }
    public class SequentialTestFixture
    {
    }

    [Collection("Sequential")]
    public class IntegrationGraphQlTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        const string graphqlEndpoint = "/graphql";

        public IntegrationGraphQlTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AuthGuestTestPassed()
        {
            var authResponse = await GuestAuthAsync();
            
            Assert.True(authResponse.authResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProductTestPassed()
        {
            // Arrange
            var result = await GuestAuthAsync();
            var client = result.client;

            var token = result.authResult.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var query = @"query Products {
    products {
        id
        code
        name
        description
        urlPicture
        price
    }
}";
            var response = await client.PostAsync(graphqlEndpoint, query.ToGraphQlContent());
            var products = await response.ConvertGraphQlResponseTo<IEnumerable<Product>>();
            // Assert
            Assert.True(!products.Any());
        }

        [Fact]
        public async Task RegisterUserTestPassed()
        {
            // Act
            var result = await RegisterAsync();

            // Assert
            Assert.True(result.registerResult);
        }

        private async Task<(HttpClient client, AuthResult authResult)> GuestAuthAsync()
        {
            var client = _factory.CreateClient();

            var mutation = @"mutation Auth {
        auth(authModel: { credentials: { email: null, password: null }, authType: GUEST }) {
            userId
            token
            roles
            isAuthorized
        }
    }";
            var response = await client.PostAsync(graphqlEndpoint, mutation.ToGraphQlContent());
            var authResp = await response.ConvertGraphQlResponseTo<AuthResult>();

            return (client, authResp);
        }

        [Fact]
        public async Task InsertProductTestPassed()
        {
            // Arrange
            var client = (await RegisterAsync()).client;

            var authMutation = @"mutation Auth {
    auth(
        authModel: {
            credentials: { email: ""user@o2.pl"", password: ""1234Abcd!"" }
            authType: LOG_IN
        }
    ) {
        userId
        token
        roles
        isAuthorized
    }
}";

            var addProductMutation = @"mutation AddProduct {
    addProduct(
        product: {
            id: 0
            code: ""tv""
            name: ""Telewizor Xiaomi""
            description: ""tv Xiaomi""
            urlPicture: ""https://sklep.tv.pl""
            price: 1400
        }
    ) {
        id
        code
        name
        description
        urlPicture
        price
    }
}";

            Product newProduct = new()
            {
                Id = 0,
                Code = "tv",
                Name = "Telewizor Xiaomi",
                Description = "tv Xiaomi",
                UrlPicture = "https://sklep.tv.pl",
                Price = 1400
            };

            // Act
            var authResponse = await client.PostAsync(graphqlEndpoint, authMutation.ToGraphQlContent());
            var token = (await authResponse.ConvertGraphQlResponseTo<AuthResult>()).Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var productsResponse = await client.PostAsync(graphqlEndpoint, addProductMutation.ToGraphQlContent());
            var responseProduct = await productsResponse.ConvertGraphQlResponseTo<Product>();

            // Assert
            newProduct.Should().BeEquivalentTo(responseProduct,
                o => o.Including(s => s.Name)
                .Including(s => s.Code)
                .Including(s => s.UrlPicture)
                .Including(s => s.Price)
                .Including(s => s.Description));
        }

        private async Task<(HttpClient client, bool registerResult)> RegisterAsync()
        {
            var content = @"mutation Register {
    register(newUser: { email: ""user@o2.pl"", password: ""1234Abcd!"" })
}";
            var userContent = content.ToGraphQlContent();
            var client = _factory.CreateClient();
            var registerResponse = await client.PostAsync(graphqlEndpoint, userContent);
            var registerResult = Convert.ToBoolean(JsonConvert.DeserializeObject<dynamic>(await registerResponse.Content.ReadAsStringAsync())?.data.register);
            
            return (client, registerResult);
        }
    }
}
