using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GetAllProductsRequest : IRequest<IEnumerable<Product>>
    {
    }
}
