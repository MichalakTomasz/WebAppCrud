using Domain.Models;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class GenerateTokenRequest : IRequest<string>
	{
        public InputRoles InputRoles { get; set; }
    }
}
