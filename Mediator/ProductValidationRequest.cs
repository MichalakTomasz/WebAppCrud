using Domain.Models;
using FluentValidation.Results;
using MediatR;
using WebAppCrud.Models;

namespace WebAppCrud.Mediator
{
	public class ProductValidationRequest : IRequest<ValidationResult>
	{
        public InputProduct ProductDto { get; set; }
    }
}
