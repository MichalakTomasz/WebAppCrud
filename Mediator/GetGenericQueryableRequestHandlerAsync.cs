using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GetGenericQueryableRequestHandlerAsync<TEntity> : IRequestHandler<GetGenericQueryableRequest<TEntity>, IQueryable<TEntity>> where TEntity : class, IIdable
	{
		private readonly IGenericRepository<TEntity> _appRepository;

		public GetGenericQueryableRequestHandlerAsync(IGenericRepository<TEntity> appRepository)
		{
			_appRepository = appRepository;
		}

		public async Task<IQueryable<TEntity>> Handle(GetGenericQueryableRequest<TEntity> request, CancellationToken cancellationToken)
			=> request.Includes?.Any() ?? false ? 
			_appRepository.GetQueryable(request.Includes) :
			_appRepository.GetQueryable();
	}
}
