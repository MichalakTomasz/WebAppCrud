using Domain.Extensions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WebAppCrud.Notifications;

namespace WebAppCrud.Mediator
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMediator _mediator;

        public RegisterRequestHandler(
            UserManager<IdentityUser> userManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<bool> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            await LogRegisterProcess(request.NewUser.Email, request.Ip);

            IdentityUser identityUser = new()
            {
                UserName = request.NewUser.Email,
                Email = request.NewUser.Email,
                PasswordHash = request.NewUser.Password,
            };
            var registerResult = await _userManager.CreateAsync(identityUser, request.NewUser.Password);

            if (!registerResult.Succeeded)
            {
                await LogRegisterFailure(identityUser.Email, "User can not registered.", request.Ip);
                return false;
            }

            var addRolesResult = await _userManager.AddToRolesAsync(identityUser, Roles.List);
            if (!addRolesResult.Succeeded)
            {
                await LogRegisterFailure(identityUser.Email, "Adding roles error.", request.Ip);
                return false;
            }

            await LogRegisterSuccess(request.NewUser.Email, Roles.List, request.Ip);

            return true;
        }

        private async Task LogRegisterProcess(string user, string ip)
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.Auth,
                Message = $"Auth user: {user} is trying to register.",
                Ip = ip
            };
            await _mediator.Publish(notification);
        }

        private async Task LogRegisterSuccess(string user, IEnumerable<string> roles, string ip)
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.Register,
                Message = $"Register user: {user} successful.{Environment.NewLine}User roles: {roles.JoinElements()}.",
                Ip = ip
            };
            await _mediator.Publish(notification);
        }

        private async Task  LogRegisterFailure(string user, string errorMessage, string ip)
        {
            LoggerNotification registerNotification = new()
            {
                NotificationType = NotificationType.RegistrationError,
                Message = $"Register user: {user} error",
                ErrorMessage = errorMessage,
                Ip = ip
            };
            await _mediator.Publish(registerNotification);
        }
    }
}
