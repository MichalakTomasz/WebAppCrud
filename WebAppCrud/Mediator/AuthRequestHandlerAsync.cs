using Domain.Extensions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WebAppCrud.Notifications;

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
            await LogAuthProcess(request.AuthModel.Credentials.Email, request.AuthModel.AuthType, request.Ip);

            switch (request.AuthModel.AuthType)
            {
                case AuthType.LogIn:
                    var user = await _userManager.FindByEmailAsync(request.AuthModel.Credentials.Email);
                    if (user == null)
                    {
                        await LogAuthFailure(request.AuthModel.Credentials.Email, CommonConsts.WrongLoginOrPasswordError, request.Ip);
                        return new AuthResult();
                    }

                    var isValidPassword = await _userManager.CheckPasswordAsync(user, request.AuthModel.Credentials.Password);
                    if (!isValidPassword)
                    {
                        await LogAuthFailure(request.AuthModel.Credentials.Email, CommonConsts.WrongLoginOrPasswordError, request.Ip);
                        return new AuthResult();
                    }

                    var roles = await _userManager.GetRolesAsync(user);

                    AuthResult loginResult = new()
                    {
                        UserId = user.Id,
                        Token = await _mediator.Send(new GenerateTokenRequest { InputRoles = new InputRoles { Roles = roles as List<string> } }),
                        Roles = roles.ToList(),
                        IsAuthorized = true
                    };

                    await LogAuthSuccess(user.Email, roles, request.Ip);
                    return loginResult;

                default:
                    AuthResult authResult = new()
                    {
                        Token = await _mediator.Send(new GenerateTokenRequest()),
                        Roles = new List<string> { CommonConsts.Guest },
                        IsAuthorized = true

                    };
                    await LogAuthSuccess("", authResult.Roles, request.Ip);
                    return authResult;
            }
        }

        private async Task LogAuthSuccess(string user, IEnumerable<string> roles, string ip)
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.Auth,
                Message = $"Auth user: {user} successful. {Environment.NewLine}User roles: {roles.JoinElements()}",
                Ip = ip
            };
            await _mediator.Publish(notification);
        }

        private async Task LogAuthFailure(string user, string errorMessage, string ip)
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.AuthError,
                Message = $"Auth user: {user} failure.",
                ErrorMessage = errorMessage,
                Ip = ip
            };
            await _mediator.Publish(notification);
        }

        private async Task LogAuthProcess(string user, AuthType authType, string ip)
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.Auth,
                Message = $"Auth user: {user} is trying to auth. {Environment.NewLine}Auth type: {authType}.",
                Ip = ip
            };
            await _mediator.Publish(notification);
        }
    }
}
