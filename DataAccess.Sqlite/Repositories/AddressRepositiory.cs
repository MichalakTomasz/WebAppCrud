using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Sqlite
{
	public class AddressRepositiory : GenericRepository<Address>, IAddressRepository
	{
        public AddressRepositiory(SqliteDbContext context) : base(context) { }
    }
}
