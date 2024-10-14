using Domain.Models;
using FluentValidation.Results;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class ProductValidationRequest : IRequest<ValidationResult>
	{
        public InputProduct InputProduct { get; set; }
    }
}
