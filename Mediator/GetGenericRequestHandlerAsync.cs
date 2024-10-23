using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
    public class GetGenericRequestHandlerAsync<TEntity> : IRequestHandler<GetGenericRequest<TEntity>, List<TEntity>> where TEntity : class, IIdable
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GetGenericRequestHandlerAsync(IGenericRepository<TEntity> genericRepository)
            => _genericRepository = genericRepository;

        public async Task<List<TEntity>> Handle(GetGenericRequest<TEntity> request, CancellationToken cancellationToken)
        => request.Includes?.Any() ?? false ? 
            await _genericRepository.GetAsync(request.Includes) :
            await _genericRepository.GetAsync();
    }
}
