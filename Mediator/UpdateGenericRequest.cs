using Domain.Interfaces;
using MediatR;

namespace WebAppCrud.Mediator
{
	public class UpdateGenericRequest<TEntity> : IRequest<TEntity> where TEntity : class, IIdable
	{
		public TEntity Entity { get; set; }
	}
}
