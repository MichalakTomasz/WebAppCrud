using Domain.Models;
using MediatR;
using WebAppCrud.Mediator;
using WebAppCrud.GraphQl.Exceptions;

namespace WebAppCrud.GraphQl.Mutations
{
	public class Mutation
	{
		[GraphQLName("addProduct")]
		//[Authorize(policy: CommonConsts.Librarian)]
		public async Task<Product> AddProductAsync(InputProduct product, [Service] IMediator mediator)
		{
			var validationResult = await mediator.Send(new ProductValidationRequest { ProductDto = product });
			if (!validationResult.IsValid)
			{
				var errorMessage = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage));
				throw new FluentValidationException(errorMessage);
			}
				

			return await mediator.Send(new AddProductRequest { Product = product });
		}
		
		[GraphQLName("updateProduct")]
		public async Task<Product> UpdateProductAsync(Product product, [Service] IMediator mediator)
			=> await mediator.Send(new UpdateProductRequest { Product = product });

		[GraphQLName("deleteProduct")]
		public async Task<bool> DeleteProductAsync(int id, [Service] IMediator mediator)
			=> await mediator.Send(new DeleteProductRequest { Id = id });
	}
}
