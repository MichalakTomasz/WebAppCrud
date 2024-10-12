using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GetAllProductsRequestHandler :
        IRequestHandler<GetAllProductsRequest, IEnumerable<Product>>
    {
        private readonly IGenericRepository<Product> _appRepository;

        public GetAllProductsRequestHandler(IGenericRepository<Product> appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
            => await _appRepository.GetAsync<Product>();
    }
}
