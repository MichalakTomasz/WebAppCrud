using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
    public class AddProductRequest : IRequest<Product>
    {
        public InputProduct Product { get; set; }
    }
}
