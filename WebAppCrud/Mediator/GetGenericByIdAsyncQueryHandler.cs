using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GetGenericByIdAsyncQueryHandler<TEntity> : IRequestHandler<GetGenericByIdRequest<TEntity>, TEntity> where TEntity : class, IIdable
    {
        private readonly IGenericRepository<TEntity> _appRepository;

        public GetGenericByIdAsyncQueryHandler(IGenericRepository<TEntity> appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<TEntity> Handle(GetGenericByIdRequest<TEntity> request, CancellationToken cancellationToken)
            => request.Includes?.Any() ?? false ?
            await _appRepository.GetAsync(request.Id, request.Includes) :
            await _appRepository.GetAsync(request.Id);
    }
}
