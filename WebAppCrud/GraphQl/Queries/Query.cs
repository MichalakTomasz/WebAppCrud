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
        public async Task<List<Product>> GetProductsAsync([Service] IMediator mediator)
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.Get,
                Message = $"{nameof(GetProductsAsync)}"
            };
            await mediator.Publish(notification);

            return await mediator.Send(new GetGenericRequest<Product>());
        }

        [UsePaging]
		[UseFiltering]
		[UseSorting]
		[GraphQLName("productsQueryable")]
		[Authorize(policy: CommonConsts.GuestPolicy)]
		public async Task<IQueryable<Product>> GetProductsQueryableAsync([Service]IMediator mediator)
		{
			LoggerNotification notification = new()
			{
				NotificationType = NotificationType.Get,
				Message = $"{nameof(GetProductsQueryableAsync)}"
			};
			await mediator.Publish(notification);

			return await mediator.Send(new GetGenericQueryableRequest<Product>());
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
