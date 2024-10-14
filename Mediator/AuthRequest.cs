using MediatR;
using WebAppCrud.Models;

namespace WebAppCrud.Mediator
{
    public class AuthRequest : IRequest<AuthResult>
    {
        public AuthModel AuthModel { get; set; }
    }
}
