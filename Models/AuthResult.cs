namespace WebAppCrud.Models
{
	public class AuthResult
	{
        public string UserId { get; set; }
		public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
