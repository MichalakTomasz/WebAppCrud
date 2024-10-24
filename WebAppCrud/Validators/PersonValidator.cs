using Domain.Models;
using FluentValidation;

namespace WebAppCrud.Validators
{
	public class PersonValidator : AbstractValidator<Person>
	{
        public PersonValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().NotNull();
            RuleFor(p => p.LastName).NotEmpty().NotNull();
            RuleFor(p => p.Address).SetValidator(new AddresValidator());
        }
    }
}
