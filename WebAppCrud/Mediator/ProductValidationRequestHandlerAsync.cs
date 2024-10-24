using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class ProductValidationRequestHandlerAsync : IRequestHandler<ProductValidationRequest, ValidationResult>
	{
		private readonly IValidator<InputProduct> _validator;

		public ProductValidationRequestHandlerAsync(IValidator<InputProduct> validator)
        {
			_validator = validator;
		}

        public  Task<ValidationResult> Handle(ProductValidationRequest request, CancellationToken cancellationToken)
			=> Task.FromResult(_validator.Validate(request.InputProduct));
	}
}
