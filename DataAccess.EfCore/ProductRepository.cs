using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.EfCore
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository

	{
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
