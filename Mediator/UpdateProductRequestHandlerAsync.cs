using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{

	public class UpdateProductRequestHandlerAsync : IRequestHandler<UpdateProductRequest, Product>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductRequestHandlerAsync(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Product> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var updateResult = await _unitOfWork.ProductRepository.UpdateAsync(request.Product);
            await _unitOfWork.CompleteAsync();

            return updateResult;
        }
    }
}
