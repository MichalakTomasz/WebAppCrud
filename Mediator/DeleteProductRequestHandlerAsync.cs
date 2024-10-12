using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class DeleteProductRequestHandlerAsync : IRequestHandler<DeleteProductRequest, bool>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteProductRequestHandlerAsync(
            IUnitOfWork	unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
		{
			await _unitOfWork.ProductRepository.DeleteAsync(request.Id);
			return (await _unitOfWork.CompleteAsync()) > 0;
		}
	}
}
