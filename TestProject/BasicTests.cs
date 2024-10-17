using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

namespace TestProject
{
	public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;
		public BasicTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/");

			// Assert
			response.EnsureSuccessStatusCode(); // Status Code 200-299
		}
	}
}