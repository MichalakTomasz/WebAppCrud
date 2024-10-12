using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.EfCore
{
	public class PersonRepository : GenericRepository<Person>, IPersonRepository
	{
        public PersonRepository(ApplicationDbContext context) : base(context) { }
    }
}
