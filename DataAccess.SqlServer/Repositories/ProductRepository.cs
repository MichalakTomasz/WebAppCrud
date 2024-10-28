using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.SqlServer.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository

	{
        public ProductRepository(SqlServerDbContext context) : base(context)
        {
            
        }
    }
}
