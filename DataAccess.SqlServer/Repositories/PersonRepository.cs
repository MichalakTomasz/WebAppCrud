using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.SqlServer.Repositories
{
	public class PersonRepository : GenericRepository<Person>, IPersonRepository
	{
        public PersonRepository(SqlServerDbContext context) : base(context) { }
    }
}
