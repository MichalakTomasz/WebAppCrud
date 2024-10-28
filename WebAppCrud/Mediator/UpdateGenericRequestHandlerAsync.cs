using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class UpdateGenericRequestHandlerAsync<TEntity> : IRequestHandler<UpdateGenericRequest<TEntity>, TEntity> where TEntity : class, IIdable
	{
		private readonly IGenericRepository<TEntity> _genericRepository;

		public UpdateGenericRequestHandlerAsync(IGenericRepository<TEntity> genericRepository)
        {
			_genericRepository = genericRepository;
		}
        public async Task<TEntity> Handle(UpdateGenericRequest<TEntity> request, CancellationToken cancellationToken)
		{
			var updateResult = await _genericRepository.UpdateAsync(request.Entity);
			await _genericRepository.SaveChangesAsync();

			return updateResult;
		}
	}
}
