using MediatR;

namespace WebAppCrud.Mediator
{
	public class DeleteProductRequest : IRequest<bool>
	{
        public int Id { get; set; }
    }
}
