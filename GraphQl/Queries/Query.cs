using Domain.Models;
using HotChocolate.Authorization;
using MediatR;
using WebAppCrud.Mediator;
using WebAppCrud.Models;
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
	}
}
