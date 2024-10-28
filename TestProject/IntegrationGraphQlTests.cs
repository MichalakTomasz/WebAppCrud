using Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>((await response.ConvertGraphQlResponse()).data.products.ToString());
            // Assert
            Assert.True(products.Count == 0);
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
            var authResp = JsonConvert.DeserializeObject<AuthResult>((await response.ConvertGraphQlResponse()).data.auth.ToString());

            return (client, authResp);
        }
    }
}
