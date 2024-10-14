using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace WebAppCrud.Mediator
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterRequestHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            IdentityUser identityUser = new()
            {
                UserName = request.NewUser.Email,
                Email = request.NewUser.Email,
                PasswordHash = request.NewUser.Password,
            };
            var registerResult = await _userManager.CreateAsync(identityUser, request.NewUser.Password);

            if (!registerResult.Succeeded)
                return false;

            var addRolesResult = await _userManager.AddToRolesAsync(identityUser, Roles.List);
            if (!addRolesResult.Succeeded)
                return false;

            return true;
        }
    }
}
