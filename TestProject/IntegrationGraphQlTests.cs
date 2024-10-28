using Domain.Models;
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

        public IntegrationGraphQlTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AuthTestPassed()
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
            var response = await client.PostAsync("/graphql", mutation.ToGraphQlContent());
            var authResp = await response.ConvertGraphQlResponseTo<AuthResult>();
            
            Assert.True(authResp.IsAuthorized);
        }
    }
}
