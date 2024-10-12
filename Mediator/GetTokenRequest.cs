using MediatR;
using WebAppCrud.Models;

namespace WebAppCrud.Mediator
{
	public class GenerateTokenRequest : IRequest<string>
	{
        public InputRoles InputRoles { get; set; }
    }
}
