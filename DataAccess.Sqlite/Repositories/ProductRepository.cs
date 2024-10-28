using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Sqlite.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository

	{
        public ProductRepository(SqliteDbContext context) : base(context)
        {
            
        }
    }
}
