using Domain.Interfaces;

namespace Domain.Models
{
    public class Person : IIdable
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public Address Address { get; set; }
	}
}
