using Domain.Models;
using FluentValidation;

namespace WebAppCrud.Validators
{
	public class AddresValidator : AbstractValidator<Address>
	{
        public AddresValidator()
        {
            RuleFor(a => a.Street).NotEmpty().NotNull();
            RuleFor(a => a.City).NotEmpty().NotNull();
        }
    }
}
