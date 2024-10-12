using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GetGenericRequestHandlerAsync<TEntity> : IRequestHandler<GetGenericRequest<TEntity>, List<TEntity>> where TEntity : class, IIdable
	{
		private readonly IGenericRepository<TEntity> _appRepository;

		public GetGenericRequestHandlerAsync(IGenericRepository<TEntity> appRepository)
		{
			_appRepository = appRepository;
		}

		public async Task<List<TEntity>> Handle(GetGenericRequest<TEntity> request, CancellationToken cancellationToken)
			=> request.Includes?.Any() ?? false ? 
			await _appRepository.GetAsync(request.Includes) :
			await _appRepository.GetAsync();
	}
}
