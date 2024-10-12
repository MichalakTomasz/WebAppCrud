using System.Text.Json.Serialization;

namespace WebAppCrud.Models
{
	public class AuthViewModel
	{
        public Credentials Credentials { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public AuthType AuthType { get; set; }
    }
}
