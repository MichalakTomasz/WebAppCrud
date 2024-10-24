using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class UpdateProductRequest : IRequest<Product>
    {
        public Product Product { get; set; }
    }
}
