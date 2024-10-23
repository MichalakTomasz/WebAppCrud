using Domain.Interfaces;
using MediatR;
namespace WebAppCrud.Mediator
{
	public class GetGenericQueryableRequest<TEntity> : IRequest<IQueryable<TEntity>> where TEntity : class, IIdable
	{
        public string[]? Includes { get; set; }
    }
}
