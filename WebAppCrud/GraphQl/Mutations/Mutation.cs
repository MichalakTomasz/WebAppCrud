using Domain.Models;
using MediatR;
using HotChocolate.Authorization;
using WebAppCrud.Mediator;
using FluentValidation;
using WebAppCrud.Exceptions;
using WebAppCrud.Notifications;

namespace WebAppCrud.GraphQl.Mutations
{
    public class Mutation
	{
		[GraphQLName("addProduct")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<Product> AddProductAsync(InputProduct product, [Service] IMediator mediator)
		{
            LoggerNotification createNotification = new()
            {
                NotificationType = NotificationType.Create,
                Message = $"{nameof(AddProductAsync)}"
            };
            await mediator.Publish(createNotification);

            var validationResult = await mediator.Send(new ProductValidationRequest { InputProduct = product });
			if (!validationResult.IsValid)
			{

                var errors = validationResult.Errors.ToString();
                LoggerNotification notification = new()
                {
                    NotificationType = NotificationType.Error,
                    Message = $"{nameof(ValidationException)}",
                    ErrorMessage = errors
                };
                await mediator.Publish(notification);

                throw new ValidationException(validationResult.Errors);
			}

            return await mediator.Send(new AddProductRequest { Product = product });
		}

		[GraphQLName("updateProduct")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<Product> UpdateProductAsync(InputProduct inputProduct, [Service] IMediator mediator)
		{
            LoggerNotification updateNotification = new()
            {
                NotificationType = NotificationType.Update,
                Message = $"{nameof(UpdateProductAsync)}"
            };
            await mediator.Publish(updateNotification);

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
        public async Task<bool> DeleteProductAsync(int id, [Service]IMediator mediator)
        {
            LoggerNotification deleteNotification = new()
            {
                NotificationType = NotificationType.Delete,
                Message = $"{nameof(DeleteProductAsync)}"
            };
            await mediator.Publish(deleteNotification);

            return await mediator.Send(new DeleteProductRequest { Id = id });
        }

        [GraphQLName("auth")]
        public async Task<AuthResult> AuthAsync(AuthModel authModel, [Service]IMediator mediator, [Service]IHttpContextAccessor httpContextAccessor)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            LoggerNotification authNotification = new()
            {
                NotificationType = NotificationType.Information,
                Message = $"{nameof(AuthAsync)}, authType: {authModel.AuthType}, user: {authModel.Credentials.Email}",
                Ip = ip
            };
            await mediator.Publish(authNotification);

            return await mediator.Send(new AuthRequest { AuthModel = authModel, Ip = ip });
        }

        [GraphQLName("register")]
        public async Task<bool> RegisterAsync(NewAppUser newUser, [Service]IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            LoggerNotification registerNotification = new()
            {
                NotificationType = NotificationType.Information,
                Message = $"{nameof(RegisterAsync)}, user: {newUser.Email}",
                Ip = ip
            };
            await mediator.Publish(registerNotification);

           return await mediator.Send(new RegisterRequest { NewUser = newUser, Ip = ip});
        }

        [GraphQLName("deleteUser")]
        [Authorize(policy: CommonConsts.AdminPolicy)]
        public async Task<bool> DeleteUserAsync(Guid userId, [Service]IMediator mediatior, IHttpContextAccessor httpContextAccessor)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            LoggerNotification deleteUserNotification = new()
            {
                NotificationType = NotificationType.Information,
                Message = $"{nameof(DeleteUserAsync)}",
                Ip = ip
            };
            await mediatior.Publish(deleteUserNotification);

            return await mediatior.Send(new DeleteUserRequest { UserId = userId, Ip = ip });
        }
    }
}
