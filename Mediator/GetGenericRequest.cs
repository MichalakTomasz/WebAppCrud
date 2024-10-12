using Domain.Interfaces;
using MediatR;
namespace WebAppCrud.Mediator
{
	public class GetGenericRequest<TEntity>: IRequest<List<TEntity>> where TEntity : class, IIdable
	{
        public string[]? Includes { get; set; }
    }
}
