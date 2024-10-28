using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.SqlServer.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class, IIdable
    {
        private readonly SqlServerDbContext _ctx;

        public GenericRepository(SqlServerDbContext context)
            => _ctx = context;

        public IQueryable<TEntity> GetQueryable()
            =>  _ctx.Set<TEntity>().AsQueryable<TEntity>();

		public IQueryable<TEntity> GetQueryable(params string[] includes)
			=>  EntityWithInclude(includes).AsQueryable();

		public async Task<List<TEntity>> GetAsync()
            => await _ctx.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<List<TEntity>> GetAsync<TProperty>(params Expression<Func<TEntity, TProperty>>[] includes)
        {
            if (includes.Any())
            {
				IQueryable<TEntity> entity = _ctx.Set<TEntity>();
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }

                return await entity.AsNoTracking().ToListAsync();
            }
            else
            {
                return await _ctx.Set<TEntity>().AsNoTracking().ToListAsync();
            }
        }

        private IQueryable<TEntity> EntityWithInclude(params string[] includes)
        {
            IQueryable<TEntity> entity = _ctx.Set<TEntity>();
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }

            return entity;
        }

		public async Task<List<TEntity>> GetAsync(params string[] includes)
		    => await EntityWithInclude(includes).AsNoTracking().ToListAsync();

		public async Task<TEntity> GetAsync(int id)
            => await _ctx.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<TEntity> GetAsync(int id, string[] includes)
            => await EntityWithInclude(includes).FirstOrDefaultAsync(p => p.Id == id);


        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var newEntity = await _ctx.AddAsync(entity);

            return newEntity.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updatedEntity = _ctx.Update(entity);

            return await Task.FromResult(updatedEntity.Entity);
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            var toDelete = await _ctx.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

            return await Task.FromResult(_ctx.Remove(toDelete).Entity);
        }

        public async Task<int> SaveChangesAsync()
            => await _ctx.SaveChangesAsync();
    }
}
