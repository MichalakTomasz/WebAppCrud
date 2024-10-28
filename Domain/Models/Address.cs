using Domain.Interfaces;

namespace Domain.Models
{
    public class Address : IIdable
	{
        public int Id { get; set; }
        public string? Street { get; set; }
        public string?  ZipCode { get; set; }
        public  string? City { get; set; }
    }
}
