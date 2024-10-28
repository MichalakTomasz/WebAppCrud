namespace Domain.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IPersonRepository PersonRepository { get; }
		IProductRepository	ProductRepository { get; }
		Task<int> CompleteAsync();
	}
}
