using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace WebAppCrud.Mediator
{
    public class GetPasswordPolicyRequestHandler : IRequestHandler<GetPasswordPolicyRequest, PasswordPolicy>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public GetPasswordPolicyRequestHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<PasswordPolicy> Handle(GetPasswordPolicyRequest request, CancellationToken cancellationToken)
        {
            PasswordPolicy passwordPolicy = new()
            {
                RequiredLength = _userManager.Options.Password.RequiredLength,
                RequireNonAlphanumeric =  _userManager.Options.Password.RequireNonAlphanumeric,
                RequireDigit = _userManager.Options.Password.RequireDigit,
                RequireLowercase =_userManager.Options.Password.RequireLowercase,
                RequireUppercase = _userManager.Options.Password.RequireUppercase
            };

            return Task.FromResult(passwordPolicy);
        }
    }
}
