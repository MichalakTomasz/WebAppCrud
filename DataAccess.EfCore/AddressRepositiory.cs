using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.EfCore
{
	public class AddressRepositiory : GenericRepository<Address>, IAddressRepository
	{
        public AddressRepositiory(ApplicationDbContext context) : base(context) { }
    }
}
