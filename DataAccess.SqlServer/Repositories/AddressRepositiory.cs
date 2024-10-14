using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.SqlServer.Repositories
{
	public class AddressRepositiory : GenericRepository<Address>, IAddressRepository
	{
        public AddressRepositiory(SqlServerDbContext context) : base(context) { }
    }
}
