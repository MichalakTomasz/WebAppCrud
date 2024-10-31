using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
    public class RegisterRequest : IRequest<bool>
    {
        public NewAppUser NewUser { get; set; }
        public string Ip { get; set; } 
    }
}
