namespace Domain.Models
{
	public class InputProduct
	{
		public int Id { get; set; }
		public string Code { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string UrlPicture { get; set; } = string.Empty;
		public double Price { get; set; }
	}
}
