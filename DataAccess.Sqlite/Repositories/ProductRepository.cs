using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Sqlite
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository

	{
        public ProductRepository(SqliteDbContext context) : base(context)
        {
            
        }
    }
}
