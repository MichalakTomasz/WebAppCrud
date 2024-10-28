using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
    public class AddProductRequestHandlerAsync : IRequestHandler<AddProductRequest, Product>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddProductRequestHandlerAsync(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Product> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.Product);
            if (product == null)
                return default;

            product.Id = 0;

            var addResult = await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return addResult;
        }
    }
}
