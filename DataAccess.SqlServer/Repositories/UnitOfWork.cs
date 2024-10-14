using Domain.Interfaces;

namespace DataAccess.SqlServer.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly SqlServerDbContext _context;

		public UnitOfWork(
			SqlServerDbContext context,
			IPersonRepository personRepository,
			IProductRepository productRepository)
        {
			_context = context;
			PersonRepository = personRepository;
			ProductRepository = productRepository;
		}
		public IPersonRepository PersonRepository { get; }

		public IProductRepository ProductRepository { get; }

		public async Task<int> CompleteAsync()
			=>  await _context.SaveChangesAsync();

		public void Dispose()
			=> _context?.Dispose();
	}
}
