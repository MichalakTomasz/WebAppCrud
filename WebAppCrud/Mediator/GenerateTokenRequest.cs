using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GenerateTokenRequest : IRequest<(string token, DateTime expiration)>
	{
        public InputRoles InputRoles { get; set; }
    }
}
