using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Sqlite.Repositories
{
	public class PersonRepository : GenericRepository<Person>, IPersonRepository
	{
        public PersonRepository(SqliteDbContext context) : base(context) { }
    }
}
