using MediatR;
using WebAppCrud.Models;

namespace WebAppCrud.Mediator
{
    public class RegisterRequest : IRequest<bool>
    {
        public NewAppUser NewUser { get; set; }
    }
}
