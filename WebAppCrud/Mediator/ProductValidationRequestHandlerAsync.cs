using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using WebAppCrud.Notifications;

namespace WebAppCrud.Mediator
{
	public class ProductValidationRequestHandlerAsync : IRequestHandler<ProductValidationRequest, ValidationResult>
	{
		private readonly IValidator<InputProduct> _validator;
        private readonly IMediator _mediator;

        public ProductValidationRequestHandlerAsync(
			IValidator<InputProduct> validator,
			IMediator mediator)
        {
			_validator = validator;
            _mediator = mediator;
        }

		public Task<ValidationResult> Handle(ProductValidationRequest request, CancellationToken cancellationToken)
		{
			var validationReslut = _validator.Validate(request.InputProduct);

			LoggerNotification notification = new()
			{
				NotificationType = NotificationType.Validate,
				Message = $"{nameof(InputProduct)} IsValid: {validationReslut.IsValid}",
				ErrorMessage = validationReslut.IsValid ? "" : validationReslut.Errors.Select(v => v.ErrorMessage).Aggregate((c, n) => c + ", " + n)
			};
			_mediator.Publish(notification);

			return Task.FromResult(validationReslut);
		}
	}
}
