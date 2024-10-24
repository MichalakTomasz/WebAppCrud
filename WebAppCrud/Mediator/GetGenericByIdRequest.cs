using Domain.Interfaces;
using MediatR;


namespace WebAppCrud.Mediator
{
    public class GetGenericByIdRequest<TEntity> : IRequest<TEntity> where TEntity : class, IIdable
    {
        public int Id { get; set; }
		public string[]? Includes { get; set; }
	}
}
