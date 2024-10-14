using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Sqlite.Repositories
{
	public class AddressRepositiory : GenericRepository<Address>, IAddressRepository
	{
        public AddressRepositiory(SqliteDbContext context) : base(context) { }
    }
}
