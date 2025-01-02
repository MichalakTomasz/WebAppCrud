using System.Runtime;

namespace Domain.Models
{
	public class AuthResult
	{
        public string? UserId { get; set; }
		public string? Token { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsAuthorized { get; set; } = false;
        public DateTime Expiration { get; set; }
    }
}
