using Domain.Models;
using FluentValidation;

namespace WebAppCrud.Validators
{
	public class InputProductValidator : AbstractValidator<InputProduct>
	{
        public InputProductValidator()
        {
            RuleFor(p => p.Code).NotEmpty().MaximumLength(10);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.UrlPicture).Matches("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_\\+.~#?&//=]*)");
        }
    }
}
