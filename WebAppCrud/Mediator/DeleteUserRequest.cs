using MediatR;

namespace WebAppCrud.Mediator
{
    public class DeleteUserRequest : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
