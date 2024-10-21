using Domain.Models;
using HotChocolate.Authorization;
using MediatR;
using WebAppCrud.Mediator;
using WebAppCrud.Notifications;

namespace WebAppCrud.GraphQl.Queries
{
    public class Query
	{
		[GraphQLName("products")]
		[Authorize(policy: CommonConsts.GuestPolicy)]
		public async Task<IEnumerable<Product>> GetProductsAsync([Service]IMediator mediator)
		{
			LoggerNotification notification = new()
			{
				NotificationType = NotificationType.Get,
				Message = $"{nameof(GetAllProductsRequest)}"
			};
			await mediator.Publish(notification);
			
			return await mediator.Send(new GetAllProductsRequest());
		}
		
		[GraphQLName("product")]
		[Authorize(policy: CommonConsts.GuestPolicy)]
		public async Task<Product> GetProductAsync(int id, [Service]IMediator mediator)
		{
			LoggerNotification notification = new()
			{
				NotificationType = NotificationType.Get,
				Message = $"{nameof(GetGenericByIdRequest<Product>)}"
			};
			await mediator.Publish(notification);

			return await mediator.Send(new GetGenericByIdRequest<Product> { Id = id });
		}

		[GraphQLName("auth")]
		public async Task<AuthResult> AuthAsync(AuthModel authModel, [Service]IMediator mediator)
			=> await mediator.Send(new AuthRequest { AuthModel = authModel });

		[GraphQLName("register")]
		public async Task<bool> RegisterAsync(NewAppUser newUser, [Service]IMediator mediator)
			=> await mediator.Send(new RegisterRequest { NewUser = newUser });

		[GraphQLName("deleteUser")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<bool> DeleteUserAsync(Guid userId, [Service]IMediator mediatior)
			=> await mediatior.Send(new DeleteUserRequest { UserId = userId });
	}
}
