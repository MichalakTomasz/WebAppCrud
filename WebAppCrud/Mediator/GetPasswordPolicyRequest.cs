using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
    public class GetPasswordPolicyRequest : IRequest<PasswordPolicy>
    {
    }
}
