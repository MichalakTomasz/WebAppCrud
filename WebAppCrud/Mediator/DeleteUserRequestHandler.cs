using MediatR;
using Microsoft.AspNetCore.Identity;

namespace WebAppCrud.Mediator
{
    public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteUserRequestHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var loggedUser = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (loggedUser == default)
                return false;

            var deleteUserResult = await _userManager.DeleteAsync(loggedUser);
            if (!deleteUserResult.Succeeded)
                return false;

            return true;
        }
    }
}
