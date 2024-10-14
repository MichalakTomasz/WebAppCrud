using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace WebAppCrud.Mediator
{
    public class AuthRequestHandlerAsync : IRequestHandler<AuthRequest, AuthResult>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMediator _mediator;

        public AuthRequestHandlerAsync(
            UserManager<IdentityUser> userManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<AuthResult> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            switch (request.AuthModel.AuthType)
            {
                case AuthType.LogIn:
                    var user = await _userManager.FindByEmailAsync(request.AuthModel.Credentials.Email);
                    if (user == null)
                        return new AuthResult();

                    var isValidPassword = await _userManager.CheckPasswordAsync(user, request.AuthModel.Credentials.Password);
                    if (!isValidPassword)
                        return new AuthResult();

                    var roles = await _userManager.GetRolesAsync(user);

                    AuthResult loginResult = new()
                    {
                        UserId = user.Id,
                        Token = await _mediator.Send(new GenerateTokenRequest { InputRoles = new InputRoles { Roles = roles as List<string> } }),
                        Roles = roles.ToList(),
                        IsAuthorized = true
                    };
                    return loginResult;

                default:
                    AuthResult authResult = new()
                    {
                        Token = await _mediator.Send(new GenerateTokenRequest()),
                        Roles = new List<string> { CommonConsts.Guest },
                        IsAuthorized = true

                    };
                    return authResult;
            }
        }
    }
}
