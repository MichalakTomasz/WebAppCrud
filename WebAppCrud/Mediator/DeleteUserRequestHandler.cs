using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WebAppCrud.Notifications;

namespace WebAppCrud.Mediator
{
    public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMediator _mediator;

        public DeleteUserRequestHandler(
            UserManager<IdentityUser> userManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }
        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            await LogDeleteUser($"Deleting user id: {request.UserId}.", request.Ip);
            var userToDelete = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (userToDelete == default)
            {
                await LogDeleteUser("Delete User error", request.Ip, $"User id: {request.UserId}, not found.");
                    return false;
            }

            var deleteUserResult = await _userManager.DeleteAsync(userToDelete);
            if (!deleteUserResult.Succeeded)
            {
                await LogDeleteUser("Delete User error", request.Ip, $"Delete from database error.");
                return false;
            }

            await LogDeleteUser($"Delete User Id{request.UserId} succeeded", request.Ip);
            return true;
        }

        private async Task LogDeleteUser(string message, string ip, string errorMessage = "")
        {
            LoggerNotification notification = new()
            {
                NotificationType = NotificationType.DeleteUser,
                Message = message,
                Ip = ip,
                ErrorMessage = errorMessage
            };
            await _mediator.Publish(notification);
        }

    }
}
