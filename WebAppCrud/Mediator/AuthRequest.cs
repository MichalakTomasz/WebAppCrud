using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
    public class AuthRequest : IRequest<AuthResult>
    {
        public AuthModel AuthModel { get; set; }
        public string Ip { get; set; }
    }
}
