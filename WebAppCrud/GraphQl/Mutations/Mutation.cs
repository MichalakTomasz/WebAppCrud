using Domain.Models;
using MediatR;
using HotChocolate.Authorization;
using WebAppCrud.Mediator;
using FluentValidation;
using WebAppCrud.Exceptions;

namespace WebAppCrud.GraphQl.Mutations
{
	public class Mutation
	{
		[GraphQLName("addProduct")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<Product> AddProductAsync(InputProduct product, [Service] IMediator mediator)
		{
			var validationResult = await mediator.Send(new ProductValidationRequest { InputProduct = product });
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}
			
			return await mediator.Send(new AddProductRequest { Product = product });
		}

		[GraphQLName("updateProduct")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<Product> UpdateProductAsync(InputProduct inputProduct, [Service] IMediator mediator)
		{
            var validationResult = await mediator.Send(new ProductValidationRequest { InputProduct = inputProduct });
			if (!validationResult.IsValid)
				throw new ValidationException(validationResult.Errors);

            var findProductResult = await mediator.Send(new GetGenericByIdRequest<Product> { Id = inputProduct.Id });
            if (findProductResult == default)
                throw new NotFoundException("Nie odnaleziono takiego produktu w bazie danych.");

            findProductResult.UrlPicture = inputProduct.UrlPicture;
            findProductResult.Price = inputProduct.Price;
            findProductResult.Name = inputProduct.Name;
            findProductResult.Description = inputProduct.Description;
            findProductResult.Code = inputProduct.Code;
            var updateResut = await mediator.Send(new UpdateProductRequest { Product = findProductResult });
			if (updateResut == default)
				throw new InternalServerError("Nie usało się zaktualizować produktu.");

			return updateResut;
		}

		[GraphQLName("deleteProduct")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<bool> DeleteProductAsync(int id, [Service] IMediator mediator)
			=> await mediator.Send(new DeleteProductRequest { Id = id });

        [GraphQLName("auth")]
        public async Task<AuthResult> AuthAsync(AuthModel authModel, [Service] IMediator mediator)
            => await mediator.Send(new AuthRequest { AuthModel = authModel });

        [GraphQLName("register")]
        public async Task<bool> RegisterAsync(NewAppUser newUser, [Service] IMediator mediator)
            => await mediator.Send(new RegisterRequest { NewUser = newUser });

        [GraphQLName("deleteUser")]
        [Authorize(policy: CommonConsts.AdminPolicy)]
        public async Task<bool> DeleteUserAsync(Guid userId, [Service] IMediator mediatior)
            => await mediatior.Send(new DeleteUserRequest { UserId = userId });
    }
}
