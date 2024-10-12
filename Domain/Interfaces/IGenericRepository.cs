using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
	public interface IGenericRepository<TEntity> where TEntity : class, IIdable
	{
		IQueryable<TEntity> GetQueryable();
		IQueryable<TEntity> GetQueryable(params string[] includes);

		Task<List<TEntity>> GetAsync();
		Task<List<TEntity>> GetAsync(params string[] includes);
		Task<TEntity> GetAsync(int id);
		Task<TEntity> GetAsync(int id, string[] includes);
		Task<List<TEntity>> GetAsync<TProperty>(params Expression<Func<TEntity, TProperty>>[] includes);

		Task<TEntity> AddAsync(TEntity entity);
		Task<TEntity> UpdateAsync(TEntity entity);
		Task<TEntity> DeleteAsync(int id);

		Task<int> SaveChangesAsync();
	}
}